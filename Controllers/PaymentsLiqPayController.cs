using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Models;
using AlexApps.Plugin.Payment.LiqPay.Services;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;

namespace AlexApps.Plugin.Payment.LiqPay.Controllers
{
    public class PaymentsLiqPayController : BasePaymentController
    {
        private readonly ILiqPayCoreService _liqPayCoreService;
        private readonly IOrderService _orderService;

        public PaymentsLiqPayController(
            ILiqPayCoreService liqPayCoreService,
            IOrderService orderService)
        {
            _liqPayCoreService = liqPayCoreService;
            _orderService = orderService;
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

            return View(liqPayGatewayModel);
        }
        
        public IActionResult ErrorResponse()
        {
            return View();
        }
        
        public async Task<IActionResult> ClientCallback(int orderId)
        {
            if (orderId == 0)
                return RedirectToAction("ErrorResponse");

            var order = await _orderService.GetOrderByIdAsync(orderId);
            order.PaymentStatus = PaymentStatus.Paid;
            order.OrderStatus = OrderStatus.Processing;
            await _orderService.UpdateOrderAsync(order);
            
            return RedirectToRoute("CheckoutCompleted", new { orderId });
        }
        
        [HttpPost]
        public async Task<IActionResult> ClientCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            var paymentApiResponse = await _liqPayCoreService.GetPaymentApiResponse(liqPayGatewayModel);
            
            return RedirectToAction("ClientCallback", new { orderId = paymentApiResponse.order_id });
        }

        [HttpPost]
        public async Task<IActionResult> ServerCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            var paymentApiResponse = await _liqPayCoreService.GetPaymentApiResponse(liqPayGatewayModel);
            
            return RedirectToAction("ClientCallback", new { orderId = paymentApiResponse.order_id });
        }
    }
}