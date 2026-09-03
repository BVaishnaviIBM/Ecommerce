using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Hubs
{
    public class OrderHub : Hub
    {
        public async Task SendOrderUpdate(
            int orderId,
            string status)
        {
            await Clients.All.SendAsync(
                "OrderUpdated",
                orderId,
                status);
        }
    }
}