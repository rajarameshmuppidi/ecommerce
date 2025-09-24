using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Repositories
{
    public class OrdersEfRepository(AppDbContext dbContext) : IOrdersRepository
    {
        private readonly AppDbContext dbContext = dbContext;

        public async Task<List<Order>> GetAllOrdersAsync(OrderParameters parameters)
        {
            var OrdersQuery = dbContext.Orders.Include(o => o.RecentCart)
                .Where(o => o.OrderedDate >= (parameters.Startdate??DateTime.MinValue) && (o.OrderedDate <= (parameters.EndDate??DateTime.MaxValue)))
                .AsQueryable();

            if(parameters.ProductId != null)
            {
                OrdersQuery = OrdersQuery.Where(o => o.ProductId == parameters.ProductId);
            }

            if (parameters.UserId != null)
            {
                OrdersQuery = OrdersQuery.Where(o => o.RecentCart.User.UserId == parameters.UserId);
            }

            if (parameters.SellerId != null)
            {
                OrdersQuery = OrdersQuery.Where(o => o.Product.Seller.UserId == parameters.SellerId);
            }

            if (parameters.OrderStatus != null)
            {
                OrdersQuery = OrdersQuery.Where(o => o.Status == parameters.OrderStatus);
            }

            if (parameters.PaymentStatus != null)
            {
                OrdersQuery = OrdersQuery.Where(o => o.PaymentStatus == parameters.PaymentStatus);
            }

            if(TryParseSortField(parameters.SortBy ?? "OrderedDate", out SortSchemes sortField))
            {
                OrdersQuery = ApplySorting(OrdersQuery, sortField, parameters.Reverse);
            }

            OrdersQuery = OrdersQuery.Skip(parameters.PageSize * parameters.PageNumber)
                                     .Take(parameters.PageSize);

            return await OrdersQuery.Include(o => o.Product).ToListAsync();
        }

        public static bool TryParseSortField(string input, out SortSchemes field)
        {
            return Enum.TryParse(input, ignoreCase: true, out field);
        }


        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            var FoundOrder = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            return FoundOrder;
        }

        public async Task<bool> CreateOrderAsync(PlaceOrderDto dto)
        {
            var product = await dbContext.Products
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);
                
            if (product == null || product.Quantity < dto.Quantity)
            {
                return false;
            }

            // Get the recent cart to verify payment details
            var recentCart = await dbContext.RecentCarts
                .FirstOrDefaultAsync(rc => rc.Id == dto.RecentCartId);
                
            if (recentCart == null)
            {
                return false;
            }

            var orderToBePlaced = new Order()
            {
                ProductId = dto.ProductId,
                RecentCartId = dto.RecentCartId,
                Status = OrderStatus.Pending,
                // Use payment details from the DTO (which comes from RecentCart)
                PaymentStatus = dto.PaymentStatus,
                PaymentMethod = dto.PaymentMethod,
                Quantity = dto.Quantity,
                DeliveryDate = null, // Will be set when shipped
                ExpectedDeliveryBy = DateTime.UtcNow.AddDays(7), // Default 7 days delivery
                OrderedDate = DateTime.UtcNow
            };

            // Update product quantity
            product.Quantity -= dto.Quantity;

            await dbContext.Orders.AddAsync(orderToBePlaced);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto)
        {
            var orderToBeChanged = await dbContext.Orders.FirstOrDefaultAsync(o => o != null && o.Id == orderId);

            if (orderToBeChanged != null)
            {
                if(dto.Status is not null) orderToBeChanged.Status = dto.Status ?? OrderStatus.Pending;
                if(dto.PaymentStatus is not null) orderToBeChanged.PaymentStatus = dto.PaymentStatus ?? PaymentStatus.Pending;
                if(dto.Qauntity is not null) orderToBeChanged.Quantity = dto.Qauntity ?? 0;
                if(dto.DeliveryDate is not null) orderToBeChanged.DeliveryDate = dto.DeliveryDate ?? DateTime.UtcNow;
                if(dto.ExpectedDeliveryBy is not null) orderToBeChanged.ExpectedDeliveryBy = dto.ExpectedDeliveryBy ?? DateTime.UtcNow.AddDays(7);

                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;

        }

        public IQueryable<Order> ApplySorting(IQueryable<Order> query, SortSchemes field, bool descending)
        {
            return field switch
            {
                SortSchemes.OrderedDate => descending ? query.OrderByDescending(o => o.OrderedDate) : query.OrderBy(o => o.OrderedDate),
                SortSchemes.ExpectedDate => descending ? query.OrderByDescending(o => o.ExpectedDeliveryBy) : query.OrderBy(o => o.ExpectedDeliveryBy),
                SortSchemes.Price => descending ? query.OrderByDescending(o => o.Product.Price) : query.OrderBy(o => o.Product.Price),
                SortSchemes.OrderStatus => descending ? query.OrderByDescending(o => o.Status) : query.OrderBy(o => o.Status),
                SortSchemes.PaymentStatus => descending ? query.OrderByDescending(o => o.PaymentStatus) : query.OrderBy(o => o.PaymentStatus),
                _ => query
            };
        }

    }
}
