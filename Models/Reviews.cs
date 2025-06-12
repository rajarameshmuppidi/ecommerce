using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class Reviews
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public Guid ProductId { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }

        public int Rating { get; set; }

        [Required]
        [StringLength(300)]
        public string Review { get; set; }

        
    }
}
