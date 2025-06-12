
using EcommercePlatform.Repositories;

namespace EcommercePlatform.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }
        public async Task<bool> DeleteCartItemByIdAsync(Guid id)
        {
            return await cartRepository.DeleteCartItemByIdAsync(id);
        }
    }
}
