using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
// SignalR Hub
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
namespace WebsiteCustomerChatMVC.SignarR
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Broadcast message to all clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}


