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

        // Payment related methods
        Task<bool> UpdatePaymentStatusAsync(Guid orderId, PaymentStatus status);
        Task<bool> UpdatePaymentMethodAsync(Guid orderId, PaymentMethod method);
        Task<PaymentStatus> GetPaymentStatusAsync(Guid orderId);
        Task<PaymentMethod> GetPaymentMethodAsync(Guid orderId);
        
        // Bulk payment updates for all orders in a recent cart
        Task<bool> UpdatePaymentStatusForCartAsync(Guid recentCartId, PaymentStatus status);
        Task<bool> UpdatePaymentMethodForCartAsync(Guid recentCartId, PaymentMethod method);

        // Get summary of seller's orders
        Task<SellerOrdersSummaryDto> GetSellerOrdersSummaryAsync(string sellerId);
    }
}
