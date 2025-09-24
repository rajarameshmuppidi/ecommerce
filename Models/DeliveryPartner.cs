using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class DeliveryPartner
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [JsonIgnore]
        public AppUser AppUser { get; set; }

        // Add any additional properties specific to DeliveryPartner
        public string VehicleNumber { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
