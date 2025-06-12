using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Utilities;

namespace EcommercePlatform.Repositories
{
    public interface IOrdersRepository
    {
        Task<Order?> GetOrderByIdAsync(Guid orderId);

        Task<List<Order>> GetAllOrdersAsync(OrderParameters parameters);

        Task<bool> CreateOrderAsync(PlaceOrderDto dto);

        Task<bool> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto);

    }
}
