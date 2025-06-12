using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using System.Collections.Generic;

namespace EcommercePlatform.Repositories
{
    public interface ICartRepository
    {
        Task<bool> AddToCartAsync(AddToCartDto dto);

        Task<List<CartItem>> GetCartItems(Guid recentCartId);

        Task<bool> UpdateRecentCartAfterOrder(Guid recentCartId);

        Task<bool> DeleteCartItemByIdAsync(Guid cartItemId);
    }
}
