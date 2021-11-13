using Nop.Core.Configuration;

namespace AlexApps.Plugin.Payment.LiqPay
{
    public class LiqPaySettings : ISettings
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string ServerCallbackUrl { get; set; }
        public string ClientCallbackUrl { get; set; }
        public bool OneClickPaymentIsAllow { get; set; }
    }
}