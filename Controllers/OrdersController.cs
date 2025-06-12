using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Services;
using EcommercePlatform.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost("{recentCartId}")]
        public async Task<IActionResult> PlaceOrder([FromRoute] Guid recentCartId)
        {
            var result  = await orderService.PlaceOrdersFromCart(recentCartId);
            return Ok(result);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpadateOrderAsync([FromRoute] Guid orderId,[FromBody] UpdateOrderDto dto)
        {
            var res = await orderService.UpdateOrderAsync(orderId, dto);

            return Ok(res);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersOfUser([FromRoute] string userId)
        {
            var parameters = new OrderParameters()
            {
                UserId = userId
            };

            return Ok(await orderService.GetOrdersOfUserAsync(parameters));
        }

        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetOrdersOfSeller([FromRoute] string sellerId)
        {
            var parameters = new OrderParameters()
            {
                SellerId = sellerId
            };

            return Ok(await orderService.GetOrdersOfUserAsync(parameters));
        }
    }
}
