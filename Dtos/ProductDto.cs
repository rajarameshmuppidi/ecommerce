using EcommercePlatform.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string ProductTitle { get; set; }


        [StringLength(200)]
        public string ProductDescription { get; set; }

        public float Price { get; set; }

        public int Quantity { get; set; }

        public string SellerId { get; set; }

        public Seller? Seller { get; set; }

        public List<Reviews> Reviews { get; set; }

    }
}
