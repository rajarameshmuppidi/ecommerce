using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommercePlatform.Models
{
    public class Seller
    {

        [Key]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        public string SellerDummy { get; set; }
    }
}
