using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Models;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace AlexApps.Plugin.Payment.LiqPay.Controllers
{
    [AutoValidateAntiforgeryToken]
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class PaymentsLiqPayConfigController : BasePaymentController
    {
        private readonly ISettingService _settingService;
        private readonly LiqPaySettings _liqPaySettings;
        private readonly IStoreContext _storeContext;
        private const string EndPointBasePath = "~/Plugins/Payments.AlexApps.LiqPay/Views/";

        public PaymentsLiqPayConfigController(
            ISettingService settingService,
            LiqPaySettings liqPaySettings,
            IStoreContext storeContext)
        {
            _settingService = settingService;
            _liqPaySettings = liqPaySettings;
            _storeContext = storeContext;
        }

        public async Task<ViewResult> Configure()
        {
            var model = new LiqPaySettingsModel(_liqPaySettings);

            return View($"{EndPointBasePath}Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(LiqPaySettingsModel model)
        {
            var settings = model.BuildLiqPaySettings();
        
            await _settingService.SaveSettingAsync(settings, (await _storeContext.GetCurrentStoreAsync()).Id);
            
            return await Configure();
        }
    }
}