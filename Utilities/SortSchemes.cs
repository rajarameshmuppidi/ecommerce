namespace EcommercePlatform
{
    public enum SortSchemes:byte
    {
        Price,
        Relevance,
        Popularity
    }

    public enum PaymentStatus:byte
    {
        Pending,
        Paid,
        
    }

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