using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EcommercePlatform.Dtos
{
    public class RegisterDeliveryPartnerDto : RegisterRequestDto
    {
        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        public string LicenseNumber { get; set; }
    }
}
