using System.Text.Json.Serialization;

namespace EcommercePlatform.Dtos
{
    public class UpdateOrderDto
    {
        public int? Qauntity { get; set; }
        public DateTime? ExpectedDeliveryBy { get; set; }
        public DateTime? DeliveryDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus? Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
