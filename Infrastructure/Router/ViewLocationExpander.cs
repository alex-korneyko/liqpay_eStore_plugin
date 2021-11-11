using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Router
{
    public class ViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var result = new List<string>(viewLocations)
            {
                "/Plugins/Payments.AlexApps.LiqPay/Views/{0}" + RazorViewEngine.ViewExtension,
                "/Plugins/Payments.AlexApps.LiqPay/Views/Shared/{0}" + RazorViewEngine.ViewExtension,
                "/Plugins/Payments.AlexApps.LiqPay/Views/{1}/{0}" + RazorViewEngine.ViewExtension,
                "/Plugins/Payments.AlexApps.LiqPay/Areas/{2}/Views/{0}" + RazorViewEngine.ViewExtension,
                "/Plugins/Payments.AlexApps.LiqPay/Areas/{2}/Views/{1}/{0}" + RazorViewEngine.ViewExtension
            };

            return result;
        }
    }
}