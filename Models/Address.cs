using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class Address
    {
        [Key] public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Vtc { get; set; }

        [Required]
        public long Pin { get; set; }

        [Required]
        [StringLength(50)]
        public string Landmark { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Apartment { get; set; }

        [Required]
        [StringLength(20)]
        public string Type { get; set; }

        public string UserId { get;set; }
        [ForeignKey(nameof(UserId)), JsonIgnore]
        public User User { get; set; }
    }
}
