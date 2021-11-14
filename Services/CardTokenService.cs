using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlexApps.Plugin.Payment.LiqPay.Domain;
using Nop.Data;

namespace AlexApps.Plugin.Payment.LiqPay.Services
{
    public class CardTokenService : ICardTokenService
    {
        private readonly IRepository<LiqPayOneClickCustomerCardToken> _cardTokenRepository;

        public CardTokenService(
            IRepository<LiqPayOneClickCustomerCardToken> cardTokenRepository)
        {
            _cardTokenRepository = cardTokenRepository;
        }

        public async Task<IList<LiqPayOneClickCustomerCardToken>> GetAllCardTokens()
        {
            return await _cardTokenRepository.GetAllAsync(_ => _);
        }

        public async Task<LiqPayOneClickCustomerCardToken> GetCardTokenByCustomerId(int customerId)
        {
            var customerCardTokens = await _cardTokenRepository
                .GetAllAsync(query => query
                    .Where(cardToken => cardToken.CustomerId == customerId));

            return customerCardTokens.FirstOrDefault();
        }

        public async Task InsertCardToken(LiqPayOneClickCustomerCardToken oneClickCustomerCardToken)
        {
            await _cardTokenRepository.InsertAsync(oneClickCustomerCardToken);
        }

        public async Task DeleteCardToken(LiqPayOneClickCustomerCardToken oneClickCustomerCardToken)
        {
            await _cardTokenRepository.DeleteAsync(oneClickCustomerCardToken);
        }
    }
}