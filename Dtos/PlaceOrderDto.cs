using EcommercePlatform.Utilities;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Dtos
{
    public class PlaceOrderDto
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public Guid RecentCartId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }
    }
}
