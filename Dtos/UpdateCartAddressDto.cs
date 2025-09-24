using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class UpdateCartAddressDto
    {
        [Required]
        public Guid CartId { get; set; }
        
        [Required]
        public Guid AddressId { get; set; }
    }
}
