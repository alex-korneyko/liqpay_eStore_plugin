using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;
using AlexApps.Plugin.Payment.LiqPay.Exceptions;
using AlexApps.Plugin.Payment.LiqPay.Models;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Orders;

namespace AlexApps.Plugin.Payment.LiqPay.Services
{
    public class LiqPayCoreService : ILiqPayCoreService
    {
        private readonly IOrderService _orderService;
        private readonly ICurrencyService _currencyService;
        private readonly LiqPaySettings _liqPaySettings;
        private readonly ICustomerService _customerService;
        private readonly ICardTokenService _cardTokenService;

        public LiqPayCoreService(
            IOrderService orderService,
            ICurrencyService currencyService,
            LiqPaySettings liqPaySettings,
            ICustomerService customerService,
            ICardTokenService cardTokenService)
        {
            _orderService = orderService;
            _currencyService = currencyService;
            _liqPaySettings = liqPaySettings;
            _customerService = customerService;
            _cardTokenService = cardTokenService;
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

            var customer = await _customerService.GetCustomerByIdAsync(order.CustomerId);

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
                result_url = _liqPaySettings.ClientCallbackUrl,
            };

            if (customer != null && _liqPaySettings.OneClickPaymentIsAllow)
            {
                paymentApiRequest.customer = customer.CustomerGuid.ToString();
                paymentApiRequest.currency = "1";
                paymentApiRequest.customer_user_id = customer.Id.ToString();
                
                var customerCardToken = await _cardTokenService.GetCardTokenByCustomerId(customer.Id);
                if (customerCardToken != null && !string.IsNullOrEmpty(customerCardToken.CardToken))
                {
                    paymentApiRequest.action = "paytoken";
                    paymentApiRequest.ip = customer.LastIpAddress;
                    paymentApiRequest.card_token = customerCardToken.CardToken;
                    paymentApiRequest.currency = "";
                }
            }

            return paymentApiRequest;
        }

        public PaymentApiResponse GetPaymentApiResponse(LiqPayGatewayModel liqPayGatewayModel)
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

        public async Task<PaymentApiResponse> RequestStatus(int orderId)
        {
            var paymentApiRequest = new PaymentApiRequest
            {
                version = 3,
                public_key = _liqPaySettings.PublicKey,
                action = "status",
                order_id = orderId.ToString()
            };

            var base64DataString = GetBase64DataString(paymentApiRequest);

            var httpClient = new HttpClient();

            var httpResponseMessage = await httpClient.GetAsync("");

            var responseMessage = await httpClient.PostAsync("https://www.liqpay.ua/api/request", 
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["data"] = base64DataString,
                    ["signature"] = GetSignature(base64DataString)
                }));
            
            var responseString = await responseMessage.Content.ReadAsStringAsync();
            var paymentApiResponse = JsonSerializer.Deserialize<PaymentApiResponse>(responseString);

            return paymentApiResponse;
        }

        public async Task SetOrderPaidSuccessfulByApiResponse(PaymentApiResponse paymentApiResponse)
        {
            var orderId = int.Parse(paymentApiResponse.order_id);
            
            if (orderId == 0) return;

            var order = await _orderService.GetOrderByIdAsync(orderId);
            order.PaymentStatus = PaymentStatus.Paid;
            order.OrderStatus = OrderStatus.Processing;
            await _orderService.UpdateOrderAsync(order);

            await _orderService.InsertOrderNoteAsync(new OrderNote
            {
                Note = $"Order successfully paid. " +
                       $"Amount: {paymentApiResponse.amount}. " +
                       $"Pay type: {paymentApiResponse.paytype}",
                OrderId = orderId,
                CreatedOnUtc = DateTime.Now
            });
        }

        public async Task SetOrderPaidFailureByApiResponse(PaymentApiResponse paymentApiResponse)
        {
            var orderId = int.Parse(paymentApiResponse.order_id);
            
            if (orderId == 0) return;

            var order = await _orderService.GetOrderByIdAsync(orderId);
            order.PaymentStatus = PaymentStatus.Pending;
            await _orderService.UpdateOrderAsync(order);
            
            var orderNote = new OrderNote
            {
                Note = $"Payment failed. " +
                       $"Error code: {paymentApiResponse.err_code}" +
                       $"Description: {paymentApiResponse.err_description}." +
                       $"Pay type: {paymentApiResponse.paytype}. " +
                       $"Amount: {paymentApiResponse.amount}. " +
                       $"Currency: {paymentApiResponse.currency}",
                OrderId = orderId,
                CreatedOnUtc = DateTime.Now
            };
            await _orderService.InsertOrderNoteAsync(orderNote);
        }
    }
}