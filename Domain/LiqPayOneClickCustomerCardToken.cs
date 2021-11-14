using Nop.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace AlexApps.Plugin.Payment.LiqPay.Domain
{
    public class LiqPayOneClickCustomerCardToken : BaseEntity
    {
        public int CustomerId { get; set; }
        public string CardToken { get; set; }
        public string CardType { get; set; }
        public string CardMask { get; set; }
    }
}