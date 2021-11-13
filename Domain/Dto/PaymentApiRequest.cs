using Nop.Core.Domain.Orders;

namespace AlexApps.Plugin.Payment.LiqPay.Domain
{
    public class PaymentApiRequest
    {
        public int version { get; set; }
        public string public_key { get; set; }
        public string action { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string order_id { get; set; }
        public string result_url { get; set; }
        public string server_url { get; set; }
        //One click payment
        public string customer { get; set; }
        public string recurringbytoken { get; set; }
        public string customer_user_id { get; set; }
        public string card_token;
    }
}