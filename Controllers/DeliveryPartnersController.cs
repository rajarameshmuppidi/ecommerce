using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommercePlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "DeliveryPartner")]
    public class DeliveryPartnersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DeliveryPartnersController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // GET: api/deliverypartners/profile
        [HttpGet("profile")]
        public async Task<ActionResult<DeliveryPartner>> GetProfile()
        {
            var userId = GetCurrentUserId();
            var deliveryPartner = await _context.DeliveryPartners
                .Include(dp => dp.AppUser)
                .FirstOrDefaultAsync(dp => dp.UserId == userId);

            if (deliveryPartner == null)
            {
                return NotFound("Delivery partner not found.");
            }

            return Ok(deliveryPartner);
        }

        // PUT: api/deliverypartners/profile
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateDeliveryPartnerDto updateDto)
        {
            var userId = GetCurrentUserId();
            var deliveryPartner = await _context.DeliveryPartners.FindAsync(userId);

            if (deliveryPartner == null)
            {
                return NotFound("Delivery partner not found.");
            }

            deliveryPartner.VehicleNumber = updateDto.VehicleNumber;
            deliveryPartner.LicenseNumber = updateDto.LicenseNumber;
            deliveryPartner.IsActive = updateDto.IsActive;

            _context.Entry(deliveryPartner).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/deliverypartners/assignments
        [HttpGet("assignments")]
        public async Task<ActionResult<IEnumerable<DeliveryPartnerAssignment>>> GetAssignments()
        {
            var userId = GetCurrentUserId();
            var assignments = await _context.DeliveryPartnerAssignments
                .Include(a => a.Order)
                    .ThenInclude(o => o.RecentCart)
                .Where(a => a.DeliveryPartnerId == userId)
                .OrderByDescending(a => a.AssignedDate)
                .ToListAsync();

            return Ok(assignments);
        }

        // GET: api/deliverypartners/assignments/{orderId}/address
        [HttpGet("assignments/{orderId}/address")]
        public async Task<ActionResult<Address>> GetOrderAddress(Guid orderId)
        {
            var userId = GetCurrentUserId();
            
            var assignment = await _context.DeliveryPartnerAssignments
                .Include(a => a.Order)
                    .ThenInclude(o => o.RecentCart)
                        .ThenInclude(rc => rc.DeliveryAddress)
                .FirstOrDefaultAsync(a => a.OrderId == orderId && a.DeliveryPartnerId == userId);

            if (assignment == null || assignment.Order?.RecentCart?.DeliveryAddress == null)
            {
                return NotFound("Delivery address not found for the specified order.");
            }

            return Ok(assignment.Order.RecentCart.DeliveryAddress);
        }

        // POST: api/deliverypartners/assign-order
        [HttpPost("assign-order")]
        
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<DeliveryPartnerAssignment>> AssignOrder(AssignOrderDto assignDto)
        {
            var order = await _context.Orders.FindAsync(assignDto.OrderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // First, verify the user exists and has the DeliveryPartner role
            var user = await _userManager.FindByIdAsync(assignDto.DeliveryPartnerId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the user has the DeliveryPartner role
            var isDeliveryPartner = await _userManager.IsInRoleAsync(user, "DeliveryPartner");
            if (!isDeliveryPartner)
            {
                return BadRequest("The specified user is not a delivery partner.");
            }

            // Verify the delivery partner exists
            var deliveryPartner = await _context.DeliveryPartners.FindAsync(assignDto.DeliveryPartnerId);
            if (deliveryPartner == null)
            {
                return NotFound("Delivery partner not found. The user must be registered as a delivery partner first.");
            }

            // Check if order is already assigned
            var existingAssignment = await _context.DeliveryPartnerAssignments
                .FirstOrDefaultAsync(a => a.OrderId == assignDto.OrderId);

            if (existingAssignment != null)
            {
                return BadRequest("This order is already assigned to a delivery partner.");
            }

            var assignment = new DeliveryPartnerAssignment
            {
                Id = Guid.NewGuid(),
                DeliveryPartnerId = assignDto.DeliveryPartnerId,
                OrderId = assignDto.OrderId,
                Status = DeliveryStatus.Assigned,
                AssignedDate = DateTime.UtcNow,
                Notes = assignDto.Notes
            };

            _context.DeliveryPartnerAssignments.Add(assignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.Id }, assignment);
        }

        // GET: api/deliverypartners/assignments/{id}
        [HttpGet("assignments/{id}")]
        public async Task<ActionResult<DeliveryPartnerAssignment>> GetAssignment(Guid id)
        {
            var userId = GetCurrentUserId();
            var assignment = await _context.DeliveryPartnerAssignments
                .Include(a => a.Order)
                .FirstOrDefaultAsync(a => a.Id == id && a.DeliveryPartnerId == userId);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        // PUT: api/deliverypartners/assignments/{id}/status
        [HttpPut("assignments/{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, UpdateOrderStatusDto statusDto)
        {
            var userId = GetCurrentUserId();
            var assignment = await _context.DeliveryPartnerAssignments
                .Include(a => a.Order)
                .FirstOrDefaultAsync(a => a.Id == id && a.DeliveryPartnerId == userId);

            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            if (assignment.Order == null)
            {
                return NotFound("Order not found.");
            }

            // Update status based on the transition
            switch (statusDto.Status.ToLower())
            {
                case "shipped":
                    assignment.Status = DeliveryStatus.PickedUp;
                    assignment.PickedUpDate = DateTime.UtcNow;
                    break;
                case "outfordelivery":
                    assignment.Status = DeliveryStatus.InTransit;
                    break;
                case "delivered":
                    assignment.Status = DeliveryStatus.Delivered;
                    assignment.DeliveredDate = DateTime.UtcNow;
                    assignment.Order.DeliveryDate = DateTime.UtcNow;
                    break;
                case "pending":
                    // Handle any pending status if needed
                    break;
                default:
                    return BadRequest($"Invalid status: {statusDto.Status}");
            }

            // Update order status - try to parse the status string to enum
            if (Enum.TryParse<OrderStatus>(statusDto.Status, true, out var status))
            {
                assignment.Order.Status = status;
            }
            else
            {
                return BadRequest($"Invalid order status: {statusDto.Status}");
            }
            
            if (!string.IsNullOrEmpty(statusDto.Notes))
            {
                assignment.Notes = statusDto.Notes;
            }

            _context.Entry(assignment).State = EntityState.Modified;
            _context.Entry(assignment.Order).State = EntityState.Modified;
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/deliverypartners/orders/{orderId}/payment-status
        [HttpPut("orders/{orderId}/payment-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid orderId, UpdatePaymentStatusDto paymentStatusDto)
        {
            var order = await _context.Orders
                .Include(o => o.RecentCart)
                .FirstOrDefaultAsync(o => o.Id == orderId);
                
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Update payment status - try to parse the status string to enum
            if (Enum.TryParse<PaymentStatus>(paymentStatusDto.PaymentStatus, true, out var paymentStatus))
            {
                order.PaymentStatus = paymentStatus;
            }
            else
            {
                return BadRequest($"Invalid payment status: {paymentStatusDto.PaymentStatus}");
            }
            
            // If payment is marked as paid, update order status to Delivered
            if (paymentStatusDto.PaymentStatus.Equals("Paid", StringComparison.OrdinalIgnoreCase))
            {
                order.Status = OrderStatus.Delivered;
                order.DeliveryDate = DateTime.UtcNow;
                
                // Also update the delivery assignment if it exists
                var assignment = await _context.DeliveryPartnerAssignments
                    .FirstOrDefaultAsync(a => a.OrderId == orderId);
                    
                if (assignment != null)
                {
                    assignment.Status = DeliveryStatus.Delivered;
                    assignment.DeliveredDate = DateTime.UtcNow;
                    _context.Entry(assignment).State = EntityState.Modified;
                }
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
