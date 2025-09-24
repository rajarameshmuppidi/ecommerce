using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EcommercePlatform.Services
{
    [Authorize(Roles = "Seller")]
    public class OrderHub : Hub
    {
        static int counter = 0;

        // Add the connected seller to a group named by their userId
        public override async Task OnConnectedAsync()
        {
            var sellerId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(sellerId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, sellerId);
            }
            await base.OnConnectedAsync();
        }

        // Remove the seller from their group on disconnect
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var sellerId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(sellerId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, sellerId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        // Send update to all connections of the seller
        public async Task SendOrderUpdate()
        {
            var sellerId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(sellerId))
            {
                await Clients.Group(sellerId).SendAsync("OrderPlaced", new { productTitle = "raja", QuantityRequested = 900 });
                await Clients.Group(sellerId).SendAsync("RecieveMessage", sellerId + counter++, "52424");
            }
        }

        public async Task TriggerUpdate()
        {
            await SendOrderUpdate();
        }
    }
}
