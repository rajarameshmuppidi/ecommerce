using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class UpdatePaymentStatusDto
    {
        [Required]
        public string PaymentStatus { get; set; }  // Values: "Pending", "Paid", "Failed", "Refunded"
    }
}
