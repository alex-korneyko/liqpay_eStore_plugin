using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace AlexApps.Plugin.Payment.LiqPay.Controllers
{
    [AutoValidateAntiforgeryToken]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class PaymentsLiqPayConfigController : BasePaymentController
    {
        private string _endPointBasePath = "~/Plugins/Payments.AlexApps.LiqPay/Views/";
        
        public IActionResult Configure()
        {

            return View($"{_endPointBasePath}Configure.cshtml");
        }
    }
}