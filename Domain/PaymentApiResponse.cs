namespace AlexApps.Plugin.Payment.LiqPay.Domain
{
    public class PaymentApiResponse
    {
        public decimal acq_id { get; set; }
        public string action { get; set; }
        public decimal agent_commission { get; set; }
        public decimal amount { get; set; }
        public decimal amount_debit { get; set; }
        public decimal amount_credit { get; set; }
        public string card_token { get; set; }
        public string completion_date { get; set; }
        public string currency { get; set; }
        public string customer { get; set; }
        public string err_code { get; set; }
        public string err_description { get; set; }
        public string info { get; set; }
        public string ip { get; set; }
        public string liqpay_order_id { get; set; }
        public string order_id { get; set; }
        public decimal payment_id { get; set; }
        public string paytype { get; set; }
        public string public_key { get; set; }
        public decimal receiver_commission { get; set; }
        public string redirect_to { get; set; } // Ссылка на которую необходимо перенаправить клиента для прохождения 3DS верификации
        public decimal sender_commission { get; set; }
        public string status { get; set; }
        public string token { get; set; }
        public string type { get; set; }
        public string err_erc { get; set; }
        public string verifycode { get; set; }
        public decimal transaction_id { get; set; }
        public int sender_card_country { get; set; }
    }
}