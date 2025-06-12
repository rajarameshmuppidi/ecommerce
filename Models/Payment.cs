namespace EcommercePlatform.Models
{
    public class Payment
    {
        public Guid Id { get; set; }

        public RecentCart RecentCart { get; set; }

        public decimal Amount { get; set; }

        public bool Paid { get; set; }

        public string Method { get; set; }

        public string PaidCurrency { get; set; }

        public DateTime PaidAt { get; set; }
    }
}
