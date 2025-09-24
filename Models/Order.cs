using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        public DateTime? DeliveryDate { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }



        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
    }
}
