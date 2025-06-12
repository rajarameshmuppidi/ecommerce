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
    }
}
