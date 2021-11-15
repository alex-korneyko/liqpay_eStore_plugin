using System.Collections.Generic;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer
{
    public static class EnResources
    {
        public static Dictionary<string, string> Resources { get; set; } = new()
        {
            [LocalizationResourceNames.PublicKey] = "Public key",
            [LocalizationResourceNames.PublicKey + ".hint"] = "Public key",
            [LocalizationResourceNames.PrivateKey] = "Private key",
            [LocalizationResourceNames.PrivateKey + ".hint"] = "Private key",
            [LocalizationResourceNames.ServerCallbackUrl] = "Callback server-server URL",
            [LocalizationResourceNames.ServerCallbackUrl + ".hint"] = "Callback server-server URL",
            [LocalizationResourceNames.ClientCallbackUrl] = "Callback client-server URL",
            [LocalizationResourceNames.ClientCallbackUrl + ".hint"] = "Callback client-server URL",
            [LocalizationResourceNames.OneClickPaymentIsAllow] = "Allow one-click payment",
            [LocalizationResourceNames.OneClickPaymentIsAllow + ".hint"] = "Allow one-click payment",
            [LocalizationResourceNames.Sandbox] = "Test mode",
            [LocalizationResourceNames.Sandbox + ".hint"] = "In testing mode, all payments are equal " +
                                                            "to 1 (UAH, RUR, USD ...) and funds are not debited from the card",
            
            [LocalizationResourceNames.YouWillBeRedirectedToLiqPay] = "You will be redirected to the website of the " +
                                                                      "LiqPay payment system",
            [LocalizationResourceNames.WhenButtonPressed] = "When you click the 'Pay' button, you will be redirected " +
                                                            "to the page of the LiqPay PrivatBank payment system",
            [LocalizationResourceNames.PaymentsList] = "You will be given several payment methods to choose from",
            [LocalizationResourceNames.PaymentDescription] = "Payment using a processing service LiqPay PrivatBank",
        };
    }
}