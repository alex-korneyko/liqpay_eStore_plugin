using AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer;
using AlexApps.Plugin.Payment.LiqPay.Infrastructure.Router;
using AlexApps.Plugin.Payment.LiqPay.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            services.AddScoped<ILocalizerService, LocalizerService>();
            services.AddScoped<ILiqPayCoreService, LiqPayCoreService>();
            services.AddScoped<ICardTokenService, CardTokenService>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });
        }

        public int Order => 1;
    }
}