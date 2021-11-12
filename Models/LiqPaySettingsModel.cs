using Nop.Web.Framework.Models;

namespace AlexApps.Plugin.Payment.LiqPay.Models
{
    public record LiqPaySettingsModel : BaseNopModel
    {
        public LiqPaySettingsModel()
        {
        }

        public LiqPaySettingsModel(LiqPaySettings liqPaySettings)
        {
            PublicKey = liqPaySettings.PublicKey;
            PrivateKey = liqPaySettings.PrivateKey;
            ServerCallbackUrl = liqPaySettings.ServerCallbackUrl;
            ClientCallbackUrl = liqPaySettings.ClientCallbackUrl;
        }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string ServerCallbackUrl { get; set; }
        public string ClientCallbackUrl { get; set; }

        public LiqPaySettings BuildLiqPaySettings()
        {
            return new LiqPaySettings
            {
                PrivateKey = PrivateKey,
                PublicKey = PublicKey,
                ClientCallbackUrl = ClientCallbackUrl,
                ServerCallbackUrl = ServerCallbackUrl
            };
        }
    }
}