namespace AlexApps.Plugin.Payment.LiqPay.Models
{
    public class LiqPayRedirectToGatewayPageModel
    {
        public LiqPayRedirectToGatewayPageModel(LiqPayGatewayModel liqPayGatewayModel)
        {
            LiqPayGatewayModel = liqPayGatewayModel;
        }

        public LiqPayGatewayModel LiqPayGatewayModel { get; set; }
        public bool OneClickPayment { get; set; }
        public string Amount { get; set; }
        public string CardDetails { get; set; }
    }
}