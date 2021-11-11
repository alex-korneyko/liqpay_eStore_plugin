using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Router
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.AlexApps.Payment.LiqPay.LiqPayGateway",
                "Plugins/PaymentsLiqPay/LiqPayGateway",
                new { controller = "PaymentsLiqPay", action = "LiqPayGateway" });
        }

        public int Priority => 1;
    }
}