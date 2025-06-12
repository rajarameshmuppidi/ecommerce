using EcommercePlatform.Dtos;
using EcommercePlatform.Models;
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
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepo;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepo)
        {
            this.userManager = userManager;
            this.tokenRepo = tokenRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            string message = "";
            var user = new AppUser
            {
                UserName = dto.Email,
                Email = dto.Email,
            };
            if (dto.Roles[0] == "Seller")
            {
                message = "Seller Created Successfully";
                user.Seller = new Seller { SellerDummy = "something for the seller cool" };
            }
            else if (dto.Roles[0] == "User")
            {
                message = "User Created Successfully";
                user.User = new User { UserDummy = "something for the user cool" };
            }

            var res= await userManager.CreateAsync(user, dto.Password);
            
            if (res.Succeeded)
            {
                res = await userManager.AddToRolesAsync(user, dto.Roles);

                if (res.Succeeded)
                {
                    return Ok(message);
                }
            }

            return BadRequest(new { Status = res.Succeeded});
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, dto.Password);

                if(checkPasswordResult)
                {
                    string jwt = this.tokenRepo.CreateJwtToken(user, [.. (await userManager.GetRolesAsync(user)).Select(r => r)]);

                    return Ok(new { Status = "Successfully logged In", jwt, ExpiresAt = DateTime.UtcNow.AddMinutes(15) });
                }
                return BadRequest("Password Is Incorrect");
            }

            return BadRequest("The User with Email Does not exist");
        }
    }


}
