using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace AlexApps.Plugin.Payment.LiqPay.Components
{
    [ViewComponent(Name = "PaymentLiqPay")]
    public class PaymentLiqPayViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}