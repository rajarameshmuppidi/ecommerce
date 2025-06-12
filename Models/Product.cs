using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string ProductTitle {  get; set; }


        [Required]
        [StringLength(200)]
        public required string ProductDescription { get; set; }

        [Required]
        public float Price { get; set; }

        [Required]
        public required int Quantity { get; set; }

        public required string SellerId { get; set; }

        [ForeignKey(nameof(SellerId))]
        public Seller? Seller { get; set; }

        public List<Reviews>? Reviews { get; set; }

        [JsonIgnore]
        public List<CartItem>? CartItems { get; set; }

    }
}
