using EcommercePlatform.Dtos;
using EcommercePlatform.Repositories;
using EcommercePlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository cartRepository;
        private readonly ICartService cartService;
        private readonly ISingleTon singleTon1;
        private readonly IScopedService scoped1;
        private readonly ITransientService transient1;
        private readonly ISingleTon singleTon2;
        private readonly IScopedService scoped2;
        private readonly ITransientService transient2;

        public CartController(ICartRepository cartRepository, ICartService cartService, ISingleTon singleTon1, IScopedService scoped1, ITransientService transient1, ISingleTon singleTon2, IScopedService scoped2, ITransientService transient2)
        {
            this.cartRepository = cartRepository;
            this.cartService = cartService;
            this.singleTon1 = singleTon1;
            this.scoped1 = scoped1;
            this.transient1 = transient1;
            this.singleTon2 = singleTon2;
            this.scoped2 = scoped2;
            this.transient2 = transient2;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCartAsync([FromBody] AddToCartDto dto)
        {
            string loggedInUser = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            dto.UserId = loggedInUser;

            var res = await cartRepository.AddToCartAsync(dto);
            //We need to pass the response to the user
            return Ok(new { IsSucceded=res });
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

        [HttpGet("user/{userId}")]
        [Authorize]
        public async Task<ActionResult> GetUserCartId([FromRoute] string userId)
        {

            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            var cartId = await cartRepository.GetUserCartId(loggedInUserId);
            return Ok(new { cartId });
        }

        [HttpGet("qtyProducts")]
        [Authorize]
        public async Task<ActionResult> GetQtyOfProducts()
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var cartId = await cartRepository.GetUserCartId(loggedInUserId);
            var res = await cartRepository.QtyOfProductsInCart(cartId ?? Guid.NewGuid());
            return Ok(res);
        }

        [HttpPut("update-address")]
        [Authorize]
        public async Task<IActionResult> UpdateCartAddress([FromBody] UpdateCartAddressDto dto)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            
            // Verify the cart belongs to the logged-in user
            var userCartId = await cartRepository.GetUserCartId(loggedInUserId);
            if (!userCartId.HasValue || userCartId.Value != dto.CartId)
            {
                return Unauthorized("You don't have permission to update this cart");
            }

            var result = await cartRepository.UpdateCartAddressAsync(dto.CartId, dto.AddressId);
            
            if (!result)
            {
                return BadRequest("Failed to update cart address. Please check if the address exists and belongs to you.");
            }

            return Ok(new { Success = true, Message = "Delivery address updated successfully" });
        }
    }
}
