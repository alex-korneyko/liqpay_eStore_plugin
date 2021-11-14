using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;
using AlexApps.Plugin.Payment.LiqPay.Models;
using Nop.Core.Domain.Orders;

namespace AlexApps.Plugin.Payment.LiqPay.Services
{
    public interface ILiqPayCoreService
    {
        string GetSignature(string base64DataString);
        string GetBase64DataString(PaymentApiRequest paymentApiRequest);
        Task<PaymentApiRequest> GetPaymentApiRequest(int orderId);
        Task<PaymentApiResponse> GetPaymentApiResponse(LiqPayGatewayModel liqPayGatewayModel);
        Task<PaymentApiResponse> RequestStatus(int orderId);
        Task SetOrderPaidByLiqPayResponse(int orderId);
    }
}