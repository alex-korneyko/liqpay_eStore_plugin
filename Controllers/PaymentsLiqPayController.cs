using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Orders;
using Nop.Web.Framework.Controllers;

namespace AlexApps.Plugin.Payment.LiqPay.Controllers
{
    public class PaymentsLiqPayController : BasePaymentController
    {
        private readonly IOrderService _orderService;

        public PaymentsLiqPayController(
            IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> LiqPayGateway(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);

            var liqPayGatewayModel = new LiqPayGatewayModel();

            return View(liqPayGatewayModel);
        }
        
        public async Task ClientCallback()
        {
            
        }

        public async Task ServerCallback(LiqPayGatewayModel liqPayGatewayModel)
        {
            
        }
    }
}