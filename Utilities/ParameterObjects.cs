namespace EcommercePlatform.Utilities
{
    public class ParameterObjects
    {
    }

    public class OrderParameters
    {
        public string? UserId { get; set; }
        public DateTime? Startdate { get; set; }
        public string? SellerId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public PaymentStatus? PaymentStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }

    public class ProductParameters
    {
        public string searchString { get; set; } = "";
        public float minPrice { get; set; } = 0;
        public float maxPrice { get; set; } = float.MaxValue;
        public SortSchemes sortBy { get; set; } = SortSchemes.Popularity;
        public bool reverse { get; set; } = false;
        public int pageSize { get; set; } = 30;
        public int pageNumber { get; set; } = 0;

        public string? sellerId { get; set; } = null;
    }
}
