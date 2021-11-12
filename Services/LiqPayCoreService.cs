using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;
using AlexApps.Plugin.Payment.LiqPay.Exceptions;
using AlexApps.Plugin.Payment.LiqPay.Models;
using Nop.Services.Directory;
using Nop.Services.Orders;

namespace AlexApps.Plugin.Payment.LiqPay.Services
{
    public class LiqPayCoreService : ILiqPayCoreService
    {
        private readonly IOrderService _orderService;
        private readonly ICurrencyService _currencyService;
        private readonly LiqPaySettings _liqPaySettings;

        public LiqPayCoreService(
            IOrderService orderService,
            ICurrencyService currencyService,
            LiqPaySettings liqPaySettings)
        {
            _orderService = orderService;
            _currencyService = currencyService;
            _liqPaySettings = liqPaySettings;
        }

        public string GetSignature(string base64DataString)
        {
            var rawString = $"{_liqPaySettings.PrivateKey}{base64DataString}{_liqPaySettings.PrivateKey}";

            using var sha1 = new SHA1Managed();

            var hash = sha1.ComputeHash(Encoding.ASCII.GetBytes(rawString));

            var base64SignatureString = Convert.ToBase64String(hash);

            return base64SignatureString;
        }

        public string GetBase64DataString(PaymentApiRequest paymentApiRequest)
        {
            var paymentRequestString = JsonSerializer.Serialize(paymentApiRequest);

            var bytes = Encoding.ASCII.GetBytes(paymentRequestString);

            var base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        public async Task<PaymentApiRequest> GetPaymentApiRequest(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new EntityNotFoundException($"Order with Id: {orderId} not found");

            var currency = await _currencyService.GetCurrencyByCodeAsync(order.CustomerCurrencyCode);

            var paymentApiRequest = new PaymentApiRequest
            {
                version = 3,
                public_key = _liqPaySettings.PublicKey,
                action = "pay",
                amount = order.OrderTotal,
                order_id = order.Id.ToString(),
                currency = currency.CurrencyCode,
                description = $"Payment from customer Id: {order.CustomerId}",
                server_url = _liqPaySettings.ServerCallbackUrl,
                result_url = _liqPaySettings.ClientCallbackUrl
            };

            return paymentApiRequest;
        }

        public async Task<PaymentApiResponse> GetPaymentApiResponse(LiqPayGatewayModel liqPayGatewayModel)
        {
            var signature = GetSignature(liqPayGatewayModel.Data);
            if (!liqPayGatewayModel.Signature.Equals(signature))
            {
                throw new Exception("Signature of LiqPay API response is invalid");
            }

            var fromBase64String = Convert.FromBase64String(liqPayGatewayModel.Data);
            var responseJsonString = Encoding.ASCII.GetString(fromBase64String);
            var paymentApiResponse = JsonSerializer.Deserialize<PaymentApiResponse>(responseJsonString);

            return paymentApiResponse;
        }
    }
}