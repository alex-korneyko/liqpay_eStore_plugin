using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

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
            OneClickPaymentIsAllow = liqPaySettings.OneClickPaymentIsAllow;
            Sandbox = liqPaySettings.Sandbox;
        }

        [NopResourceDisplayName("Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.PublicKey")]
        public string PublicKey { get; set; }
        [NopResourceDisplayName("Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.PrivateKey")]
        public string PrivateKey { get; set; }
        [NopResourceDisplayName("Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.ServerCallbackUrl")]
        public string ServerCallbackUrl { get; set; }
        [NopResourceDisplayName("Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.ClientCallbackUrl")]
        public string ClientCallbackUrl { get; set; }
        [NopResourceDisplayName("Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.OneClickPaymentIsAllow")]
        public bool OneClickPaymentIsAllow { get; set; }
        [NopResourceDisplayName("Plugins.AlexApps.Payment.LiqPay.ConfigModel.Fields.Sandbox")]
        public bool Sandbox { get; set; }

        public LiqPaySettings BuildLiqPaySettings()
        {
            return new LiqPaySettings
            {
                PrivateKey = PrivateKey,
                PublicKey = PublicKey,
                ClientCallbackUrl = ClientCallbackUrl,
                ServerCallbackUrl = ServerCallbackUrl,
                OneClickPaymentIsAllow = OneClickPaymentIsAllow,
                Sandbox = Sandbox
            };
        }
    }
}