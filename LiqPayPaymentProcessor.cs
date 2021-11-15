using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;
using Nop.Services.Plugins;

namespace AlexApps.Plugin.Payment.LiqPay
{
    public class LiqPayPaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly IWebHelper _webHelper;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly LiqPaySettings _liqPaySettings;
        private readonly ILocalizerService _localizerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;

        public LiqPayPaymentProcessor(
            IWebHelper webHelper,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor,
            IActionContextAccessor actionContextAccessor,
            IStoreContext storeContext,
            ISettingService settingService,
            LiqPaySettings liqPaySettings,
            ILocalizerService localizerService,
            IGenericAttributeService genericAttributeService,
            IWorkContext workContext,
            ILocalizationService localizationService)
        {
            _webHelper = webHelper;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _actionContextAccessor = actionContextAccessor;
            _storeContext = storeContext;
            _settingService = settingService;
            _liqPaySettings = liqPaySettings;
            _localizerService = localizerService;
            _genericAttributeService = genericAttributeService;
            _workContext = workContext;
            _localizationService = localizationService;
        }

        /// <summary>
        /// This method is always invoked right before a customer places an order. Use it when you need to process
        /// a payment before an order is stored into database. For example, capture or authorize credit card.
        /// Usually this method is used when a customer is not redirected to third-party site for
        /// completing a payment and all payments are handled on your site (for example, PayPal Direct).
        /// </summary>
        /// <param name="processPaymentRequest"></param>
        /// <returns></returns>
        public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            await _genericAttributeService.SaveAttributeAsync(
                await _workContext.GetCurrentCustomerAsync(),
                LiqPayDefaults.ProcessingOrderGuid,
                processPaymentRequest.OrderGuid,
                processPaymentRequest.StoreId);
            
            return new ProcessPaymentResult
            {
                NewPaymentStatus = PaymentStatus.Pending
            };
        }

        /// <summary>
        /// This method is invoked right after a customer places an order. Usually this method is used when you need
        /// to redirect a customer to a third-party site for completing a payment (for example, PayPal Standard).
        /// </summary>
        /// <param name="postProcessPaymentRequest"></param>
        /// <returns></returns>
        public async Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            
            var routeUrl = urlHelper.RouteUrl(
                "Plugin.AlexApps.Payment.LiqPay.LiqPayGateway",
                new {orderId = postProcessPaymentRequest.Order.Id},
                (await _storeContext.GetCurrentStoreAsync()).SslEnabled ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

            if (_httpContextAccessor.HttpContext != null) 
                _httpContextAccessor.HttpContext.Response.Redirect(routeUrl);
            else
                throw new Exception("Failed getting HttpContext");
        }

        /// <summary>
        /// You can put any logic here. For example, hide this payment method if all products in the cart
        /// are downloadable. Or hide this payment method if current customer is from certain country
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// You can return any additional handling fees which will be added to an order total
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        public Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return Task.FromResult<decimal>(0);
        }

        /// <summary>
        /// Some payment gateways allow you to authorize payments before they're captured. It allows store owners to
        /// review order details before the payment is actually done. In this case you just authorize a payment
        /// in ProcessPayment or PostProcessPayment method, and then just capture it.
        /// In this case a Capture button will be visible on the order details page in admin area.
        /// Note that an order should be already authorized and SupportCapture property should return true.
        /// </summary>
        /// <param name="capturePaymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            return Task.FromResult(new CapturePaymentResult { Errors = new[] { "Capture method not supported" } });
        }

        /// <summary>
        /// This method allows you make a refund. In this case a Refund button will be visible on the order details
        /// page in admin area. Note that an order should be paid, and SupportRefund
        /// or SupportPartiallyRefund property should return true
        /// </summary>
        /// <param name="refundPaymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            return Task.FromResult(new RefundPaymentResult { Errors = new[] { "Refund method not supported" } });
        }

        /// <summary>
        /// This method allows you void an authorized but not captured payment.
        /// In this case a Void button will be visible on the order details page in admin area.
        /// Note that an order should be authorized and SupportVoid property should return true.
        /// </summary>
        /// <param name="voidPaymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return Task.FromResult(new VoidPaymentResult { Errors = new[] { "Void method not supported" } });
        }

        /// <summary>
        /// Use this method to process recurring payments.
        /// </summary>
        /// <param name="processPaymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult {  Errors = new[] { "Recurring payment not supported" } });
        }

        /// <summary>
        /// Use this method to cancel recurring payments.
        /// </summary>
        /// <param name="cancelPaymentRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return Task.FromResult(new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } });
        }

        /// <summary>
        /// Usually this method is used when it redirects a customer to a third-party site for completing a payment.
        /// If the third party payment fails this option will allow customers
        /// to attempt the order again later without placing a new order.
        /// CanRePostProcessPayment should return true to enable this feature.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// is used in the public store to validate customer input. It returns a list of warnings
        /// (for example, a customer did not enter his credit card name). If your payment method does not ask
        /// the customer to enter additional information, then the ValidatePaymentForm should return an empty list
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            return Task.FromResult<IList<string>>(new List<string>());
        }

        /// <summary>
        /// method is used in the public store to parse customer input, such as credit card information.
        /// This method returns a ProcessPaymentRequest object with parsed customer input
        /// (for example, credit card information). If your payment method does not ask the customer
        /// to enter additional information, then GetPaymentInfo will return an empty ProcessPaymentRequest object
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            return Task.FromResult(new ProcessPaymentRequest());
        }

        public string GetPublicViewComponentName()
        {
            return "PaymentLiqPay";
        }

        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return await _localizationService.GetResourceAsync(LocalizationResourceNames.PaymentDescription);
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentsLiqPayConfig/Configure";
        }

        public override async Task InstallAsync()
        {
            var storeLocation = _webHelper.GetStoreLocation();
            
            _liqPaySettings.ClientCallbackUrl = storeLocation + "Plugins/PaymentsLiqPay/ClientCallback";
            _liqPaySettings.ServerCallbackUrl = storeLocation + "Plugins/PaymentsLiqPay/ServerCallback";

            await _settingService.SaveSettingAsync(_liqPaySettings, (await _storeContext.GetCurrentStoreAsync()).Id);

            await _localizerService.SetLocaleResources();

            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await _localizerService.RemoveLocaleResources();
            
            await base.UninstallAsync();
        }

        public bool SupportCapture => false;
        public bool SupportPartiallyRefund => false;
        public bool SupportRefund => false;
        public bool SupportVoid => false;
        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;
        public PaymentMethodType PaymentMethodType => PaymentMethodType.Redirection;
        public bool SkipPaymentInfo => false;
    }
}