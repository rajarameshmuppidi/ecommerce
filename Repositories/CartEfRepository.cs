
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
        public async Task<RecentCart?> AddToCartAsync(AddToCartDto dto)
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
                var ExistingProductInCart = await dbContext.CartItems.FirstOrDefaultAsync(ci => ci.ProductId == dto.ProductId && ci.RecentCartId == recentCartOfUser.Id);

                if(ExistingProductInCart!=null)
                {
                    if(dto.Quantity == 0)
                    {
                        dbContext.CartItems.Remove(ExistingProductInCart);
                        await dbContext.SaveChangesAsync();
                    }else
                    {
                        ExistingProductInCart.Quantity = dto.Quantity;
                        await dbContext.SaveChangesAsync();
                    }
                }
                else { 
                    var cartItem = new CartItem { RecentCartId = recentCartOfUser.Id, ProductId = dto.ProductId, Quantity = dto.Quantity };
                    await dbContext.CartItems.AddAsync(cartItem);
                    await dbContext.SaveChangesAsync();
                }
                    
            }

            return recentCartOfUser;
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
            var cartItems = await dbContext.CartItems.Where(c => c.RecentCartId == recentCartId).Include(ci => ci.Product).ToListAsync();

            return cartItems;
        }

        public async Task<Guid?> GetUserCartId(string userId)
        {
            var recentCartOfUser = await dbContext.RecentCarts.Where(r => r.UserId == userId && !r.Ordered).FirstOrDefaultAsync();
            if (recentCartOfUser == null)
            {
                var newRecentCart = new RecentCart() { Ordered = false, UserId = userId, UpdateDate = DateTime.UtcNow };
                await dbContext.RecentCarts.AddAsync(newRecentCart);
                if (await dbContext.SaveChangesAsync() > 0)
                {
                    recentCartOfUser = newRecentCart;
                }
            }
            return recentCartOfUser.Id;
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

        public async Task<bool> IsCartOrdered(Guid cartId)
        {
            var cart = await this.dbContext.RecentCarts.FirstOrDefaultAsync(rc => rc.Id == cartId);
            if(cart != null)
            {
                return cart.Ordered;
            }
            return false;
        }

        public async Task<Dictionary<Guid, int>> QtyOfProductsInCart(Guid cartId)
        {
            Dictionary<Guid, int> val = await dbContext.CartItems.Where(ci => ci.RecentCartId == cartId).Select(ci => new { ci.ProductId, ci.Quantity }).ToDictionaryAsync(ci => ci.ProductId, ci => ci.Quantity);
            return val;
        }

        public async Task<bool> UpdateCartAddressAsync(Guid cartId, Guid addressId)
        {
            var cart = await dbContext.RecentCarts.FindAsync(cartId);
            if (cart == null)
                return false;

            // Verify the address exists and belongs to the same user
            var address = await dbContext.Addresses.FindAsync(addressId);
            if (address == null || address.UserId != cart.UserId)
                return false;

            cart.DeliveryAddressId = addressId;
            var result = await dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
