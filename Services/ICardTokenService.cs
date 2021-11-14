using System.Collections.Generic;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;

namespace AlexApps.Plugin.Payment.LiqPay.Services
{
    public interface ICardTokenService
    {
        Task<IList<LiqPayOneClickCustomerCardToken>> GetAllCardTokens();
        Task<LiqPayOneClickCustomerCardToken> GetCardTokenByCustomerId(int customerId);
        Task InsertCardToken(LiqPayOneClickCustomerCardToken oneClickCustomerCardToken);
        Task DeleteCardToken(LiqPayOneClickCustomerCardToken oneClickCustomerCardToken);
    }
}