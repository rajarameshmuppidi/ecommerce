using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Models
{
    public class DeliveryPartnerAssignment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string DeliveryPartnerId { get; set; }

        [ForeignKey(nameof(DeliveryPartnerId))]
        [JsonIgnore]
        public DeliveryPartner DeliveryPartner { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        [JsonIgnore]
        public Order Order { get; set; }

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

        public DateTime? PickedUpDate { get; set; }

        public DateTime? DeliveredDate { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeliveryStatus Status { get; set; } = DeliveryStatus.Assigned;

        public string Notes { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeliveryStatus
    {
        Assigned,    // Order has been assigned to delivery partner
        PickedUp,    // Delivery partner has picked up the order
        InTransit,   // Order is out for delivery
        Delivered,   // Order has been delivered
        Cancelled,   // Delivery was cancelled
        Failed       // Delivery attempt failed
    }
}
