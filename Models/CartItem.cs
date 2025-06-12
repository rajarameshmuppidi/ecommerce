using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class CartItem
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RecentCartId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(RecentCartId))]
        public RecentCart RecentCart { get; set; }

        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        [Required, NotNull]
        public int Quantity { get; set; }
    }
}
