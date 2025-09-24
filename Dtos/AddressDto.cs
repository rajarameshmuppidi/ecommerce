using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class AddressDto
    {
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
    }
}
