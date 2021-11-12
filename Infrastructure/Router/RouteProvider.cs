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
            
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.AlexApps.Payment.LiqPay.ClientCallback",
                "Plugins/PaymentsLiqPay/ClientCallback",
                new { controller = "PaymentsLiqPay", action = "ClientCallback" });
            
            endpointRouteBuilder.MapControllerRoute(
                "Plugin.AlexApps.Payment.LiqPay.ServerCallback",
                "Plugins/PaymentsLiqPay/ServerCallback",
                new { controller = "PaymentsLiqPay", action = "ServerCallback" });
        }

        public int Priority => 1;
    }
}