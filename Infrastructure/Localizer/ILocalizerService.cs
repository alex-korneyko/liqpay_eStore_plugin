using System.Threading.Tasks;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer
{
    public interface ILocalizerService
    {
        Task SetLocaleResources();
        Task RemoveLocaleResources();
    }
}