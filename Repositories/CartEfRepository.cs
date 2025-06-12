
using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EcommercePlatform.Repositories
{

    public class CartEfRepository : ICartRepository
    {
        private readonly AppDbContext dbContext;

        public CartEfRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<bool> AddToCartAsync(AddToCartDto dto)
        {
            var recentCartOfUser = await dbContext.RecentCarts.Where(r => r.UserId == dto.UserId && !r.Ordered).FirstOrDefaultAsync();

            if (recentCartOfUser == null)
            {
                var newRecentCart = new RecentCart() { Ordered = false, UserId = dto.UserId, UpdateDate = DateTime.UtcNow };
                await dbContext.RecentCarts.AddAsync(newRecentCart);
                if (await dbContext.SaveChangesAsync() > 0)
                {
                    recentCartOfUser = newRecentCart;
                }
            }

            if (recentCartOfUser != null)
            {
                var cartItem = new CartItem { RecentCartId = recentCartOfUser.Id, ProductId = dto.ProductId, Quantity = dto.Quantity };
                await dbContext.CartItems.AddAsync(cartItem);
                int res = await dbContext.SaveChangesAsync();
                if (res > 0) { return true; }
            }

            return false;
        }

        public async Task<bool> DeleteCartItemByIdAsync(Guid cartItemId)
        {
            var itemToBeRemoved = await dbContext.CartItems.FirstOrDefaultAsync(x => x.Id == cartItemId);
            if (itemToBeRemoved != null)
            {
                dbContext.CartItems.Remove(itemToBeRemoved);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<CartItem>> GetCartItems(Guid recentCartId)
        {
            var cartItems = await dbContext.CartItems.Where(c => c.RecentCartId == recentCartId).ToListAsync();

            return cartItems;
        }

        public async Task<bool> UpdateRecentCartAfterOrder(Guid recentCartId)
        {
            var recentCart = await this.dbContext.RecentCarts.FirstOrDefaultAsync(r => r.Id == recentCartId);
            if (recentCart != null)
            {
                recentCart.Ordered = true;
                return true;
            }
            return false;
        }
    }
}
