using Microsoft.AspNetCore.Identity;

namespace EcommercePlatform.Utilities
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
