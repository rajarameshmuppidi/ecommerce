using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AddressesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AddressesController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Addresses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        // GET: api/Addresses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
            {
                return NotFound();
            }

            return address;
        }

        // POST: api/Addresses
        [HttpPost]
        public async Task<ActionResult<Address>> PostAddress(AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not authenticated");
            }
            
            var address = new Address
            {
                Vtc = addressDto.Vtc,
                Pin = addressDto.Pin,
                Landmark = addressDto.Landmark,
                PhoneNumber = addressDto.PhoneNumber,
                Apartment = addressDto.Apartment,
                Type = addressDto.Type,
                UserId = userId
            };
            
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
        }

        // PUT: api/Addresses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAddress(Guid id, AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

            if (existingAddress == null)
            {
                return NotFound();
            }

            // Update the address properties from DTO
            existingAddress.Vtc = addressDto.Vtc;
            existingAddress.Pin = addressDto.Pin;
            existingAddress.Landmark = addressDto.Landmark;
            existingAddress.PhoneNumber = addressDto.PhoneNumber;
            existingAddress.Apartment = addressDto.Apartment;
            existingAddress.Type = addressDto.Type;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Addresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
                
            if (address == null)
            {
                return NotFound();
            }

            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AddressExists(Guid id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
