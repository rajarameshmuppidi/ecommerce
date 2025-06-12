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
                .Where(o => o.RecentCart.UpdateDate >= (parameters.Startdate??DateTime.MinValue) && o.RecentCart.UpdateDate <= parameters.EndDate)
                .AsQueryable();

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

            OrdersQuery = OrdersQuery.Skip(parameters.PageSize * parameters.PageNumber)
                                     .Take(parameters.PageSize);

            return await OrdersQuery.ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            var FoundOrder = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

            return FoundOrder;
        }

        public async Task<bool> CreateOrderAsync(PlaceOrderDto dto)
        {
            var orderToBePlaced = new Order()
            {
                ProductId = dto.ProductId,
                RecentCartId = dto.RecentCartId,
                Status = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                Quantity = dto.Quantity,
                DeliveryDate = DateTime.UtcNow,
                ExpectedDeliveryBy = DateTime.UtcNow.AddDays(8),
                OrderedDate = DateTime.UtcNow
            };

            await dbContext.Orders.AddAsync(orderToBePlaced);
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
    }
}
