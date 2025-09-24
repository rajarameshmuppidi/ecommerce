using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class UpdateDeliveryPartnerDto
    {
        [Required]
        public string VehicleNumber { get; set; }
        
        [Required]
        public string LicenseNumber { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
