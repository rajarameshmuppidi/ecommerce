// hi from raja ramesh muppidi
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Repositories;
using EcommercePlatform.Utilities;

namespace EcommercePlatform.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrdersRepository orderRepository;
        private readonly ICartRepository cartRepository;
        private readonly AppDbContext dbContext;
        private readonly IHubContext<OrderHub> hubContext;

        public OrderService(IOrdersRepository orderRepository, ICartRepository cartRepository, AppDbContext dbContext, IHubContext<OrderHub> hubContext)
        {
            this.orderRepository = orderRepository;
            this.cartRepository = cartRepository;
            this.dbContext = dbContext;
            this.hubContext = hubContext;
        }

        public async Task<IEnumerable<Order>> GetOrdersOfSellerAsync(OrderParameters parameters)
        {
            var result = await orderRepository.GetAllOrdersAsync(parameters);

            return result;
        }

        public async Task<IEnumerable<Order>> GetOrdersOfUserAsync(OrderParameters parameters)
        {
            var result = await orderRepository.GetAllOrdersAsync(parameters);

            return result;
        }

        public async Task<string> PlaceOrdersFromCart(Guid recentCartId)
        {
            if (await cartRepository.IsCartOrdered(recentCartId))
            {
                return "CartEmpty";
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Get the cart to access payment details
                var cart = await dbContext.RecentCarts.FindAsync(recentCartId);
                if (cart == null)
                {
                    return "CartNotFound";
                }

                // Verify cart has a delivery address
                if (cart.DeliveryAddressId == null)
                {
                    return "DeliveryAddressRequired";
                }

                var ItemsInCart = await cartRepository.GetCartItems(recentCartId);
                foreach (var Item in ItemsInCart)
                {
                    // Check product quantity before placing order
                    var product = await dbContext.Products.FindAsync(Item.ProductId);
                    if (product == null || product.Quantity < Item.Quantity)
                    {
                        await transaction.RollbackAsync();
                        return "InsufficientStock";
                    }
                    
                    var ItemtoBeOrdered = new PlaceOrderDto
                    {
                        ProductId = Item.ProductId,
                        Quantity = Item.Quantity,
                        RecentCartId = recentCartId,
                        // Use payment details from the cart
                        PaymentStatus = cart.PaymentStatus,
                        PaymentMethod = cart.PaymentMethod
                    };
                    
                    product.Quantity -= Item.Quantity;
                    var orderCreated = await orderRepository.CreateOrderAsync(ItemtoBeOrdered);
                    if (!orderCreated)
                    {
                        await transaction.RollbackAsync();
                        return "InsufficientStock";
                    }
                    // Notify seller via SignalR
                    await hubContext.Clients.Group(product.SellerId).SendAsync(
                        "OrderPlaced",
                        new {
                            ProductTitle = product.ProductTitle,
                            QuantityRequested = Item.Quantity
                        }
                    );
                }
                await cartRepository.UpdateRecentCartAfterOrder(recentCartId);
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.InnerException?.Message ?? ex.Message;
            }

        }

        public async Task<bool> UpdateOrderAsync(Guid orderId, UpdateOrderDto dto)
        {
            return await orderRepository.UpdateOrderAsync(orderId, dto);
        }

        public async Task<bool> UpdatePaymentStatusAsync(Guid orderId, PaymentStatus status)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.PaymentStatus = status;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePaymentMethodAsync(Guid orderId, PaymentMethod method)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
                return false;

            order.PaymentMethod = method;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PaymentStatus> GetPaymentStatusAsync(Guid orderId)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            return order.PaymentStatus;
        }

        public async Task<PaymentMethod> GetPaymentMethodAsync(Guid orderId)
        {
            var order = await dbContext.Orders.FindAsync(orderId);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            return order.PaymentMethod;
        }

        public async Task<bool> UpdatePaymentStatusForCartAsync(Guid recentCartId, PaymentStatus status)
        {
            try
            {
                // First verify the cart exists
                var cartExists = await dbContext.RecentCarts.FirstOrDefaultAsync(rc => rc.Id == recentCartId);
                if (cartExists==null)
                    return false;


                // Update all orders for this cart in a single query
                var orders = await dbContext.Orders
                    .Where(o => o.RecentCartId == recentCartId)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    order.PaymentStatus = status;
                }

                cartExists.PaymentStatus = status;

                var updatedCount = await dbContext.SaveChangesAsync();
                return updatedCount > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to inject ILogger)
                Console.WriteLine($"Error updating payment status for cart {recentCartId}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePaymentMethodForCartAsync(Guid recentCartId, PaymentMethod method)
        {
            try
            {
                // First verify the cart exists
                var cartExists = await dbContext.RecentCarts.FirstOrDefaultAsync(rc => rc.Id == recentCartId);
                if (cartExists==null)
                    return false;

                // Update all orders for this cart in a single query
                var orders = await dbContext.Orders
                    .Where(o => o.RecentCartId == recentCartId)
                    .ToListAsync();

                foreach (var order in orders)
                {
                    order.PaymentMethod = method;
                }

                cartExists.PaymentMethod = method;

                var updatedCount = await dbContext.SaveChangesAsync();
                return updatedCount > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to inject ILogger)
                Console.WriteLine($"Error updating payment method for cart {recentCartId}: {ex.Message}");
                return false;
            }
        }

        public async Task<SellerOrdersSummaryDto> GetSellerOrdersSummaryAsync(string sellerId)
        {
            var now = DateTime.UtcNow;
            
            // Get all orders for the seller's products
            var ordersQuery = dbContext.Orders
                .Include(o => o.Product)
                .Where(o => o.Product.SellerId == sellerId);

            // Count orders by status
            var pending = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Pending);
            var shipped = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Shipped);
            var outForDelivery = await ordersQuery.CountAsync(o => o.Status == OrderStatus.OutForDelivery);
            var delivered = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Delivered);
            
            // Count late deliveries (expected delivery date is in the past and status is not delivered)
            var lateDeliveries = await ordersQuery
                .CountAsync(o => o.ExpectedDeliveryBy < now && 
                               o.Status != OrderStatus.Delivered);
            
            // Calculate date ranges for monthly and yearly earnings
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayOfYear = new DateTime(now.Year, 1, 1);
            
            // Base query for seller's products
            var sellerProductsQuery = dbContext.Products
                .Where(p => p.SellerId == sellerId)
                .Select(p => p.Id);
            
            // Base query for paid orders of seller's products
            var paidOrdersQuery = dbContext.Orders
                .Where(o => o.PaymentStatus == PaymentStatus.Paid && 
                           sellerProductsQuery.Contains(o.ProductId));
            
            // Total settled earnings (all time)
            var settledEarnings = await paidOrdersQuery
                .SumAsync(o => (decimal)o.Quantity * (decimal)o.Product.Price);
                
            // Monthly settled earnings
            var settledEarningsThisMonth = await paidOrdersQuery
                .Where(o => o.OrderedDate >= firstDayOfMonth)
                .SumAsync(o => (decimal)o.Quantity * (decimal)o.Product.Price);
                
            // Yearly settled earnings
            var settledEarningsThisYear = await paidOrdersQuery
                .Where(o => o.OrderedDate >= firstDayOfYear)
                .SumAsync(o => (decimal)o.Quantity * (decimal)o.Product.Price);

            // Calculate pending amount (for orders with pending payment)
            var pendingAmount = await dbContext.Orders
                .Where(o => o.PaymentStatus == PaymentStatus.Pending && 
                           sellerProductsQuery.Contains(o.ProductId))
                .SumAsync(o => (decimal)o.Quantity * (decimal)o.Product.Price);

            // Count total products for this seller
            var totalProducts = await dbContext.Products
                .CountAsync(p => p.SellerId == sellerId);

            return new SellerOrdersSummaryDto
            {
                Pending = pending,
                Shipped = shipped,
                OutForDelivery = outForDelivery,
                Delivered = delivered,
                LateDeliveries = lateDeliveries,
                SettledEarnings = settledEarnings,
                SettledEarningsThisMonth = settledEarningsThisMonth,
                SettledEarningsThisYear = settledEarningsThisYear,
                PendingAmount = pendingAmount,
                TotalProducts = totalProducts
            };
        }
    }
}
