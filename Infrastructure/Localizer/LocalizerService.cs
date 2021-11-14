using System.Threading.Tasks;
using Nop.Services.Localization;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer
{
    public class LocalizerService : ILocalizerService
    {
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;

        public LocalizerService(ILocalizationService localizationService, ILanguageService languageService)
        {
            _localizationService = localizationService;
            _languageService = languageService;
        }
        
        public async Task SetLocaleResources()
        {
            await _localizationService.AddLocaleResourceAsync(EnResources.Resources);
            
            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                switch (languageName)
                {
                    case "ru":
                        await _localizationService.AddLocaleResourceAsync(RuResources.Resources, language.Id);
                        break;
                    case "uk":
                        await _localizationService.AddLocaleResourceAsync(UaResources.Resources, language.Id);
                        break;
                }
            }
        }
        
        public async Task RemoveLocaleResources()
        {
            await _localizationService.DeleteLocaleResourcesAsync(LocalizationResourceNames.GetValues());

            var allLanguagesAsync = await _languageService.GetAllLanguagesAsync();
            
            foreach (var language in allLanguagesAsync)
            {
                var languageName = _languageService.GetTwoLetterIsoLanguageName(language);
                
                switch (languageName)
                {
                    case "ru":
                    case "uk":
                        await _localizationService.DeleteLocaleResourcesAsync(LocalizationResourceNames.GetValues(), language.Id);
                        break;
                }
            }
        }
    }
}