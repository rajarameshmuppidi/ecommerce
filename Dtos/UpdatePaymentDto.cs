using System.Text.Json.Serialization;
using EcommercePlatform.Utilities;

namespace EcommercePlatform.Dtos
{
    public class UpdatePaymentDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus? Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod? Method { get; set; }
    }
}
