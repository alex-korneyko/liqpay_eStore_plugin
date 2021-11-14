using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;
using AlexApps.Plugin.Payment.LiqPay.Models;

namespace AlexApps.Plugin.Payment.LiqPay.Services
{
    public interface ILiqPayCoreService
    {
        string GetSignature(string base64DataString);
        string GetBase64DataString(PaymentApiRequest paymentApiRequest);
        Task<PaymentApiRequest> GetPaymentApiRequest(int orderId);
        PaymentApiResponse GetPaymentApiResponse(LiqPayGatewayModel liqPayGatewayModel);
        Task<PaymentApiResponse> RequestStatus(int orderId);
        Task SetOrderPaidSuccessfulByApiResponse(PaymentApiResponse orderId);
        Task SetOrderPaidFailureByApiResponse(PaymentApiResponse orderId);
    }
}