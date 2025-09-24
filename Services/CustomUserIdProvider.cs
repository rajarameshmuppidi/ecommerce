using EcommercePlatform.Models;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EcommercePlatform.Services
{
    public class CustomUserIdProvider: IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
