#nullable enable
using System;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Models;
using AlexApps.Plugin.Payment.LiqPay.Services;
using Microsoft.AspNetCore.Mvc;
using Nito.AsyncEx;
using Nop.Core;
using Nop.Services.Common;
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

        public PaymentsLiqPayController(
            ILiqPayCoreService liqPayCoreService,
            IOrderService orderService,
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            IStoreContext storeContext)
        {
            _liqPayCoreService = liqPayCoreService;
            _orderService = orderService;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _storeContext = storeContext;
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
        
        public IActionResult ErrorResponse()
        {
            return View();
        }
        
        public async Task<IActionResult> ClientCallback(int orderId)
        {
            if (orderId != 0) 
                return RedirectToRoute("CheckoutCompleted", new { orderId });
            
            var processedOrderGuid = await _genericAttributeService.GetAttributeAsync<string>(
                await _workContext.GetCurrentCustomerAsync(),
                LiqPayDefaults.ProcessingOrderGuid,
                (await _storeContext.GetCurrentStoreAsync()).Id);

            if (string.IsNullOrEmpty(processedOrderGuid))
                return RedirectToRoute("CheckoutCompleted", new { orderId });
                
            var orderByGuid = await _orderService.GetOrderByGuidAsync(Guid.Parse(processedOrderGuid));
            
            var statusApiResponse = await _liqPayCoreService.RequestStatus(orderByGuid.Id);
            
            if (statusApiResponse.status == "success")
            {
                await _liqPayCoreService.SetOrderPaidByLiqPayResponse(orderByGuid.Id);
            }

            return RedirectToRoute("CheckoutCompleted", new { orderByGuid.Id });
        }
        
        [HttpPost]
        public async Task<IActionResult> ClientCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            var paymentApiResponse = await _liqPayCoreService.GetPaymentApiResponse(liqPayGatewayModel);
            if (paymentApiResponse.status == "success")
            {
                await _liqPayCoreService.SetOrderPaidByLiqPayResponse(int.Parse(paymentApiResponse.order_id));
            }
            
            return RedirectToAction("ClientCallback", new { orderId = paymentApiResponse.order_id });
        }

        [HttpPost]
        public async Task ServerCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            var paymentApiResponse = await _liqPayCoreService.GetPaymentApiResponse(liqPayGatewayModel);
            if (paymentApiResponse.status == "success")
            {
                await _liqPayCoreService.SetOrderPaidByLiqPayResponse(int.Parse(paymentApiResponse.order_id));
            }
        }
    }
}