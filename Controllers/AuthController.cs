using EcommercePlatform.Data;
using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
using EcommercePlatform.Services;
using EcommercePlatform.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext _context;
        private readonly ITokenRepository tokenRepo;
        private readonly ISingleTon singleTon1;
        private readonly IScopedService scoped1;
        private readonly ITransientService transient1;
        private readonly ISingleTon singleTon2;
        private readonly IScopedService scoped2;
        private readonly ITransientService transient2;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepo, ISingleTon singleTon1, IScopedService scoped1, ITransientService transient1, ISingleTon singleTon2, IScopedService scoped2, ITransientService transient2, AppDbContext context)
        {
            this.userManager = userManager;
            this.tokenRepo = tokenRepo;
            this.singleTon1 = singleTon1;
            this.scoped1 = scoped1;
            this.transient1 = transient1;
            this.singleTon2 = singleTon2;
            this.scoped2 = scoped2;
            this.transient2 = transient2;
            _context = context;
        }

        [HttpPost("register")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            return await RegisterUser(dto);
        }

        [HttpPost("register/delivery-partner")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> RegisterDeliveryPartner([FromBody] RegisterDeliveryPartnerDto dto)
        {
            // Set the role to DeliveryPartner
            dto.Roles = new List<string> { "DeliveryPartner" };
            
            // Create a RegisterRequestDto from the delivery partner DTO
            var registerDto = new RegisterRequestDto
            {
                Email = dto.Email,
                Password = dto.Password,
                ConfirmPassword = dto.ConfirmPassword,
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Roles = dto.Roles,
                // Store the delivery partner specific data in AdditionalData
                AdditionalData = new Dictionary<string, string>
                {
                    { "VehicleNumber", dto.VehicleNumber },
                    { "LicenseNumber", dto.LicenseNumber }
                }
            };
            
            return await RegisterUser(registerDto);
        }

        private async Task<IActionResult> RegisterUser(RegisterRequestDto dto)
        {
            if (dto.Roles == null || !dto.Roles.Any())
            {
                return BadRequest(new { Success = false, Message = "At least one role is required" });
            }

            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber
            };

            // Handle different user types
            switch (dto.Roles[0])
            {
                case "Seller":
                    user.Seller = new Seller { SellerDummy = "something for the seller cool" };
                    break;
                    
                case "DeliveryPartner":
                    // Get delivery partner data from AdditionalData
                    if (dto.AdditionalData == null || 
                        !dto.AdditionalData.TryGetValue("VehicleNumber", out var vehicleNumber) ||
                        !dto.AdditionalData.TryGetValue("LicenseNumber", out var licenseNumber))
                    {
                        return BadRequest(new { Success = false, Message = "Vehicle number and license number are required for delivery partners" });
                    }

                    user.DeliveryPartner = new DeliveryPartner
                    {
                        VehicleNumber = vehicleNumber,
                        LicenseNumber = licenseNumber,
                        IsActive = true
                    };
                    break;
                    
                case "User":
                    user.User = new User { UserDummy = "something for the user cool" };
                    break;
                    
                default:
                    return BadRequest(new { Success = false, Message = "Invalid role specified" });
            }

            // Create the user
            var result = await userManager.CreateAsync(user, dto.Password);
            
            if (!result.Succeeded)
            {
                return BadRequest(new { Success = false, Errors = result.Errors.Select(e => e.Description) });
            }

            // Add roles to the user
            var roleResult = await userManager.AddToRolesAsync(user, dto.Roles);
            if (!roleResult.Succeeded)
            {
                // If adding roles fails, delete the user to maintain data consistency
                await userManager.DeleteAsync(user);
                return BadRequest(new { Success = false, Errors = roleResult.Errors.Select(e => e.Description) });
            }

            return Ok(new 
            { 
                Success = true, 
                Message = $"{dto.Roles[0]} created successfully",
                UserId = user.Id,
                Email = user.Email,
                Roles = dto.Roles
            });
        }


        [HttpPost("login")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            
            if (user == null)
            {
                return NotFound(new { Success = false, Code = "UserNotFound" });
            }

            var checkPasswordResult = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!checkPasswordResult)
            {
                return BadRequest(new { Success = false, Code = "IncorrectPassword" });
            }

            var roles = await userManager.GetRolesAsync(user);
            if (roles == null || !roles.Any())
            {
                return BadRequest(new { Success = false, Code = "NoRolesAssigned" });
            }

            var jwt = this.tokenRepo.CreateJwtToken(user, roles.ToList());
            return Ok(new 
            { 
                Success = true, 
                Token = jwt, 
                ExpiresAt = DateTime.UtcNow.AddMinutes(15), 
                Roles = roles,
                UserId = user.Id,
                Email = user.Email
            });
        }


        [HttpGet("Dummny1")]
        public Dictionary<String,Guid> Get1()
        {
            Dictionary<String, Guid> result = new Dictionary<string, Guid>()
            {
                {"singleton1" , singleTon1.GetGuid() },
                {"singleton2", singleTon2.GetGuid() },
                {"scoped1", scoped1.GetGuid() },
                {"scoped2", scoped2.GetGuid() },
                {"transient1", transient1.GetGuid() },
                {"transient2", transient2.GetGuid() }
            };

            return result;
        }

        [HttpGet("Dummny2")]
        public Dictionary<String, Guid> Get2()
        {
            Dictionary<String, Guid> result = new Dictionary<string, Guid>()
            {
                {"singleton1" , singleTon1.GetGuid() },
                {"singleton2", singleTon2.GetGuid() },
                {"scoped1", scoped1.GetGuid() },
                {"scoped2", scoped2.GetGuid() },
                {"transient1", transient1.GetGuid() },
                {"transient2", transient2.GetGuid() }
            };

            return result;
        }

    }


}
