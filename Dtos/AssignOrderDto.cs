using System;
using System.ComponentModel.DataAnnotations;

namespace EcommercePlatform.Dtos
{
    public class AssignOrderDto
    {
        [Required]
        public Guid OrderId { get; set; }
        
        [Required]
        public string DeliveryPartnerId { get; set; }
        
        public string Notes { get; set; }
    }
}
