using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePlatform.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public Guid RecentCartId { get; set; }
        [ForeignKey(nameof(RecentCartId))]
        public RecentCart RecentCart { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime OrderedDate { get; set; }


        [Required]
        public DateTime ExpectedDeliveryBy { get; set; }

        [Required]
        public DateTime? DeliveryDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
