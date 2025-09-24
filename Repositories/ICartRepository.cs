using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using System.Collections.Generic;

namespace EcommercePlatform.Repositories
{
    public interface ICartRepository
    {
        Task<Dictionary<Guid, int>> QtyOfProductsInCart(Guid cartId);
        Task<bool> IsCartOrdered(Guid cartId);
        Task<Guid?> GetUserCartId(string userId);

        Task<RecentCart?> AddToCartAsync(AddToCartDto dto);

        Task<List<CartItem>> GetCartItems(Guid recentCartId);

        Task<bool> UpdateRecentCartAfterOrder(Guid recentCartId);

        Task<bool> DeleteCartItemByIdAsync(Guid cartItemId);

        Task<bool> UpdateCartAddressAsync(Guid cartId, Guid addressId);
    }
}
