using System.Collections.Generic;
using System.Linq;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer
{
    public static class LocalizationResourceNames
    {
        public static string PublicKey => "Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.PublicKey";
        public static string PrivateKey => "Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.PrivateKey";
        public static string ServerCallbackUrl => "Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.ServerCallbackUrl";
        public static string ClientCallbackUrl => "Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.ClientCallbackUrl";
        public static string OneClickPaymentIsAllow => "Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.OneClickPaymentIsAllow";

        //Messages
        public static string YouWillBeRedirectedToLiqPay =>
            "Plugins.AlexApps.Payment.LiqPay.Messages.YouWillBeRedirectedToLiqPay";

        public static string WhenButtonPressed => "Plugins.AlexApps.Payment.LiqPay.Messages.WhenButtonPressed";
        public static string PaymentsList => "Plugins.AlexApps.Payment.LiqPay.Messages.PaymentsList";
        
        public static IList<string> GetValues()
        {
            return typeof(LocalizationResourceNames).GetMethods().Select(methodInfo => methodInfo.Name).ToList();
        }
    }
}