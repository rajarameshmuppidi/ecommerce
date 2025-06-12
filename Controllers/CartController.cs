using EcommercePlatform.Dtos;
using EcommercePlatform.Repositories;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartService cartService;

        public CartController(ICartRepository cartRepository, ICartService cartService)
        {
            this.cartRepository = cartRepository;
            this.cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCartAsync([FromBody] AddToCartDto dto)
        {
            var res = await cartRepository.AddToCartAsync(dto);
            return Ok(res);
        }

        [HttpGet("{recentCartId}")]
        public async Task<IActionResult> GetCartItemsOfAUser([FromRoute] Guid recentCartId)
        {
            var res = await cartRepository.GetCartItems(recentCartId);
            return Ok(res);
        }

        [HttpDelete("{cartId}")]
        public async Task<ActionResult<bool>> DeleteWithIdAsync([FromRoute] Guid cartId)
        {
            return Ok(await cartService.DeleteCartItemByIdAsync(cartId));
        }
    }
}
