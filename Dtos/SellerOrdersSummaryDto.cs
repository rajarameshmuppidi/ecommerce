using System.Text.Json.Serialization;

namespace EcommercePlatform.Dtos
{
    public class SellerOrdersSummaryDto
    {
        public int Pending { get; set; }
        public int Shipped { get; set; }
        public int OutForDelivery { get; set; }
        public int Delivered { get; set; }
        public int LateDeliveries { get; set; }
        public decimal SettledEarnings { get; set; }
        public decimal SettledEarningsThisMonth { get; set; }
        public decimal SettledEarningsThisYear { get; set; }
        public decimal PendingAmount { get; set; }
        public int TotalProducts { get; set; }
    }
}
