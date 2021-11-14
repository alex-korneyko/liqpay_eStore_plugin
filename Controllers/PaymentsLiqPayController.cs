#nullable enable
using System;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;
using AlexApps.Plugin.Payment.LiqPay.Models;
using AlexApps.Plugin.Payment.LiqPay.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;

namespace AlexApps.Plugin.Payment.LiqPay.Controllers
{
    public class PaymentsLiqPayController : BasePaymentController
    {
        private readonly ILiqPayCoreService _liqPayCoreService;
        private readonly IOrderService _orderService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ILogger _logger;

        public PaymentsLiqPayController(
            ILiqPayCoreService liqPayCoreService,
            IOrderService orderService,
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ILogger logger)
        {
            _liqPayCoreService = liqPayCoreService;
            _orderService = orderService;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
            _logger = logger;
        }

        public async Task<IActionResult> LiqPayGateway(int orderId)
        {
            var paymentApiRequest = await _liqPayCoreService.GetPaymentApiRequest(orderId);

            var base64DataString = _liqPayCoreService.GetBase64DataString(paymentApiRequest);

            var liqPayGatewayModel = new LiqPayGatewayModel
            {
                Data = base64DataString,
                Signature = _liqPayCoreService.GetSignature(base64DataString)
            };

            var liqPayRedirectToGatewayPageModel = new LiqPayRedirectToGatewayPageModel(liqPayGatewayModel);

            return View(liqPayRedirectToGatewayPageModel);
        }
        
        public IActionResult ErrorResponse(PaymentErrorModel paymentErrorModel)
        {
            return View(paymentErrorModel);
        }
        
        public async Task<IActionResult> ClientCallback(PaymentApiResponse paymentApiResponse)
        {
            if (string.IsNullOrEmpty(paymentApiResponse.order_id))
            {
                var processedOrderGuid = await _genericAttributeService.GetAttributeAsync<string>(
                    await _workContext.GetCurrentCustomerAsync(),
                    LiqPayDefaults.ProcessingOrderGuid,
                    (await _storeContext.GetCurrentStoreAsync()).Id);

                if (string.IsNullOrEmpty(processedOrderGuid))
                {
                    var paymentErrorModel = new PaymentErrorModel{Description = "An error occurred during checkout"};
                    return RedirectToAction("ErrorResponse", new {paymentErrorModel});
                }

                var orderByGuid = await _orderService.GetOrderByGuidAsync(Guid.Parse(processedOrderGuid));
            
                paymentApiResponse = await _liqPayCoreService.RequestStatus(orderByGuid.Id);
            }

            switch (paymentApiResponse.status)
            {
                case "error":
                    var description = $"Payment error! Code: {paymentApiResponse.err_code}, " +
                                  $"Description: {paymentApiResponse.err_description}";
                    
                    var logMessage = $"{description}. Order ID: {paymentApiResponse.order_id}";
                    await _logger.ErrorAsync(logMessage, customer: await _workContext.GetCurrentCustomerAsync());
                    
                    return RedirectToAction("ErrorResponse", new { description });
                case "success":
                    await _liqPayCoreService.SetOrderPaidSuccessfulByApiResponse(paymentApiResponse);
                    break;
                case "sandbox":
                    await _liqPayCoreService.SetOrderPaidSuccessfulByApiResponse(paymentApiResponse, true);
                    break;
                case "failure":
                    await _liqPayCoreService.SetOrderPaidFailureByApiResponse(paymentApiResponse);
                    break;
            }

            return RedirectToRoute("CheckoutCompleted", new { orderId = paymentApiResponse.order_id });
        }
        
        [HttpPost]
        public IActionResult ClientCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            return RedirectToAction("ClientCallback", new { liqPayGatewayModel });
        }

        [HttpPost]
        public async Task ServerCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            var paymentApiResponse = _liqPayCoreService.GetPaymentApiResponse(liqPayGatewayModel);
            switch (paymentApiResponse.status)
            {
                case "success":
                    await _liqPayCoreService.SetOrderPaidSuccessfulByApiResponse(paymentApiResponse);
                    break;
                case "sandbox":
                    await _liqPayCoreService.SetOrderPaidSuccessfulByApiResponse(paymentApiResponse, true);
                    break;
                case "failure":
                    await _liqPayCoreService.SetOrderPaidFailureByApiResponse(paymentApiResponse);
                    break;
            }
        }
    }
}