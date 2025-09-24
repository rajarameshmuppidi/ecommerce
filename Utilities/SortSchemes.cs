using System.Text.Json.Serialization;

namespace EcommercePlatform
{
    public enum SortSchemes:byte
    {
        OrderedDate,
        ExpectedDate,
        Price,
        OrderStatus,
        PaymentStatus,
        Relevance,
        Popularity
    }

    public enum PaymentStatus:byte
    {
        Pending,
        Paid,
        
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod:byte
    {
        Cod,
        Upi,
        DebitCard,
        CreditCard,
        Wallet
    }

    public enum OrderStatus:byte
    {
        Pending,
        Delivered,
        Shipped,
        OutForDelivery
    }
}