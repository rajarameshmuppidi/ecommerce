using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class AppUser : IdentityUser
    {
        [JsonIgnore]
        public virtual User User { get; set; }
        
        [JsonIgnore]
        public virtual Seller Seller { get; set; }
        
        [JsonIgnore]
        public virtual DeliveryPartner DeliveryPartner { get; set; }
    }
}
