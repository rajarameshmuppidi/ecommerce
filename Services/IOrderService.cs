using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;

namespace EcommercePlatform.Services
{
    public interface IOrderService
    {

        public Task<string> PlaceOrdersFromCart(Guid recentCartId);

        public Task<bool> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto);

        public Task<IEnumerable<Order>> GetOrdersOfUserAsync(OrderParameters parameters);

        public Task<IEnumerable<Order>> GetOrdersOfSellerAsync(OrderParameters parameters);
    }
}
