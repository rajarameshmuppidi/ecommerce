using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;

namespace EcommercePlatform.Utilities
{
    public class ParameterObjects
    {
    }

    public class OrderParameters
    {
        public string? UserId { get; set; }
        public Guid? ProductId { get; set; }
        public DateTime? Startdate { get; set; }
        public string? SellerId { get; set; }

        public OrderStatus? OrderStatus { get; set; }
        public DateTime? EndDate { get; set; } = DateTime.MaxValue;
        public PaymentStatus? PaymentStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; } = null;
        public bool Reverse { get; set; } = false;
    }

    
    public class ProductParameters
    {
        public string searchString { get; set; } = "";
        public float minPrice { get; set; } = 0;
        public float maxPrice { get; set; } = float.MaxValue;
        public SortSchemes sortBy { get; set; } = SortSchemes.Popularity;
        public bool reverse { get; set; } = false;
        public int pageSize { get; set; } = 30;
        public int pageNumber { get; set; } = 1;

        public string? sellerId { get; set; } = null;
    }

    public class QueryParameters<T>
    {
        public Expression<Func<T, bool>>? Filter { get; set; }

        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }

        public List<Expression<Func<T, object>>>? Includes { get; set; }

        public int? Page { get; set; } = 1;

        public int? PageSize { get; set; } = 10;

        public Expression<Func<T, bool>>? MinMaxCondition { get; set; }
    }
}
