using EcommercePlatform.Dtos;

using EcommercePlatform.Models;
using EcommercePlatform.Services;
using EcommercePlatform.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var result = await orderService.PlaceOrdersFromCart(recentCartId);
            if (result == "InsufficientStock")
                return BadRequest(new { Status = "InsufficientStock", Message = "One or more products in the cart are out of stock." });
            return Ok(new { Status = result });
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateOrderAsync([FromRoute] Guid orderId, [FromBody] UpdateOrderDto dto)
        {
            var res = await orderService.UpdateOrderAsync(orderId, dto);

            return Ok(res);
        }

        [HttpPut("{orderId}/payment/status")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentStatus([FromRoute] Guid orderId, [FromBody] UpdatePaymentDto dto)
        {
            if (dto.Status == null)
                return BadRequest("Payment status is required.");

            var result = await orderService.UpdatePaymentStatusAsync(orderId, dto.Status.Value);
            if (!result)
                return NotFound($"Order with ID {orderId} not found.");

            return Ok(new { Success = true, Message = "Payment status updated successfully." });
        }

        [HttpPut("{orderId}/payment/method")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentMethod([FromRoute] Guid orderId, [FromBody] UpdatePaymentDto dto)
        {
            if (dto.Method == null)
                return BadRequest("Payment method is required.");

            var result = await orderService.UpdatePaymentMethodAsync(orderId, dto.Method.Value);
            if (!result)
                return NotFound($"Order with ID {orderId} not found.");

            return Ok(new { Success = true, Message = "Payment method updated successfully." });
        }

        [HttpGet("{orderId}/payment/status")]
        [Authorize]
        public async Task<IActionResult> GetPaymentStatus([FromRoute] Guid orderId)
        {
            try
            {
                var status = await orderService.GetPaymentStatusAsync(orderId);
                return Ok(new { Status = status });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{orderId}/payment/method")]
        [Authorize]
        public async Task<IActionResult> GetPaymentMethod([FromRoute] Guid orderId)
        {
            try
            {
                var method = await orderService.GetPaymentMethodAsync(orderId);
                return Ok(new { Method = method });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        //}

        [HttpPut("cart/{recentCartId}/payment/status")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentStatusForCart(
            [FromRoute] Guid recentCartId, 
            [FromBody] UpdatePaymentDto dto)
        {
            if (dto.Status == null)
                return BadRequest("Payment status is required.");

            var result = await orderService.UpdatePaymentStatusForCartAsync(recentCartId, dto.Status.Value);
            if (!result)
                return NotFound($"No orders found for cart ID {recentCartId}.");

            return Ok(new { Success = true, Message = "Payment status updated for all orders in the cart." });
        }

        [HttpPut("cart/{recentCartId}/payment/method")]
        [Authorize]
        public async Task<IActionResult> UpdatePaymentMethodForCart(
            [FromRoute] Guid recentCartId, 
            [FromBody] UpdatePaymentDto dto)
        {
            if (dto.Method == null)
                return BadRequest("Payment method is required.");

            var result = await orderService.UpdatePaymentMethodForCartAsync(recentCartId, dto.Method.Value);
            if (!result)
                return NotFound($"No orders found for cart ID {recentCartId}.");

            return Ok(new { Success = true, Message = "Payment method updated for all orders in the cart." });
        }

        [HttpGet("seller/summary")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetSellerOrdersSummary()
        {
            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(sellerId))
                return Unauthorized();

            var summary = await orderService.GetSellerOrdersSummaryAsync(sellerId);
            return Ok(summary);
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<IActionResult> GetOrdersOfUser(
            [FromQuery] DateTime? startDate, 
            [FromQuery] string? sellerId,
            [FromQuery] OrderStatus? orderStatus,
            [FromQuery] DateTime? endDate,
            [FromQuery] PaymentStatus? paymentStatus,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            string[] roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();

            if (startDate is null) startDate = DateTime.MinValue;
            if (endDate is null) endDate = DateTime.MaxValue;

            var parameters = new OrderParameters()
            {
                Startdate = startDate,
                //SellerId = sellerId,
                OrderStatus = orderStatus,
                EndDate = endDate,
                PaymentStatus = paymentStatus,
                PageNumber = pageNumber,
                PageSize = pageSize,
                UserId = loggedInUserId,
            };

            return Ok(await orderService.GetOrdersOfUserAsync(parameters));
        }

        [HttpGet("seller/")]
        [Authorize(Roles ="Seller")]
        public async Task<IActionResult> GetOrdersOfSeller(
            [FromQuery] DateTime? startDate,
            [FromQuery] string? sellerId,
            [FromQuery] OrderStatus? orderStatus,
            [FromQuery] DateTime? endDate,
            [FromQuery] PaymentStatus? paymentStatus,
            [FromQuery] int? pageNumber,
            [FromQuery] bool? reverse,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? productId = null,
            [FromQuery] string? sortBy = null
            
            )
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            string[] roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();

            var parameters = new OrderParameters()
            {
                Startdate = startDate??DateTime.MinValue,
                //SellerId = sellerId,
                OrderStatus = orderStatus,
                EndDate = endDate??DateTime.MaxValue,
                PaymentStatus = paymentStatus,
                PageNumber = pageNumber ?? 0,
                PageSize = pageSize,
                SellerId = loggedInUserId,
                ProductId = productId,
                SortBy = sortBy,
                Reverse = reverse ?? false
            };

            return Ok(await orderService.GetOrdersOfUserAsync(parameters));
        }
    }
}
