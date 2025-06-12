using EcommercePlatform.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class ProductUpdateDto
    {

        [StringLength(100)]
        public string? ProductTitle { get; set; }


        [StringLength(200)]
        public string? ProductDescription { get; set; }

        public float? Price { get; set; }

        public int? Quantity { get; set; }

    }
}
