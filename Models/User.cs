using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AppUser AppUser { get; set; }

        public string UserDummy { get; set; }

        [JsonIgnore]
        public List<Reviews> Reviews { get; set; }

        [JsonIgnore]
        public List<RecentCart> RecentCarts { get; set; }
    }
}
