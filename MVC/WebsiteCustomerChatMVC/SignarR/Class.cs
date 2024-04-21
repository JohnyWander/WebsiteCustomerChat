using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
// SignalR Hub
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace WebsiteCustomerChatMVC.SignarR
{
    public class ConnectedClient
    {
        public string ConnectionID;
    }

    public class ChatHub : Hub
    {
        IDictionary<string, ConnectedClient> headers;


        public override async Task OnConnectedAsync()
        {
            string clientID = Context.ConnectionId;

            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            // Use the connectionId to identify the client
            Console.WriteLine($"Client disconnected: {connectionId}");
            await base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessage(string user, string message)
        {
            // Broadcast message to all clients
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }


        
    }
}


