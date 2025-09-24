using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        public string Status { get; set; }  // Values: "Pending", "Processing", "Shipped", "OutForDelivery", "Delivered", "Cancelled"
        
        public string Notes { get; set; }
    }
}
