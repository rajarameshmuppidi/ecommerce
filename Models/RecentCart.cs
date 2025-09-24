using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class RecentCart
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public DateTime UpdateDate { get; set; }

        [Required]
        public bool Ordered { get; set; }

        [JsonIgnore]
        public List<Order>? Orders { get; set; }

        [JsonIgnore]
        public List<CartItem>? CartItems { get; set; }

        public Guid? DeliveryAddressId { get; set; }
        [ForeignKey(nameof(DeliveryAddressId))]
        [JsonIgnore]
        public Address? DeliveryAddress { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cod;
    }
}
