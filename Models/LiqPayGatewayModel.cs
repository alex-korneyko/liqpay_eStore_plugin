using Nop.Core.Domain.Orders;
using Nop.Web.Framework.Models;

namespace AlexApps.Plugin.Payment.LiqPay.Models
{
    public record LiqPayGatewayModel : BaseNopModel
    {
        public string Data { get; set; }
        public string Signature { get; set; }
    }
}