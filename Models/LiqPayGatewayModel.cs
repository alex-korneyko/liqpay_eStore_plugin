using Nop.Core.Domain.Orders;

namespace AlexApps.Plugin.Payment.LiqPay.Models
{
    public class LiqPayGatewayModel
    {
        public Order Order{ get; set; }
        public string Data { get; set; }
        public string Signature { get; set; }
    }
}