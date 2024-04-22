using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
// SignalR Hub
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using System.Diagnostics;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    

    public class ChatHub : Hub
    {
      

        

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            string clientID = Context.ConnectionId;
           

            ChatEvents.FireConnected(this, new UserConnectedEventArgs(connectionId));

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            await base.OnDisconnectedAsync(exception);
        }

        

        public async Task SendMessage(string message)
        {
            ChatEvents.FireText(this, new UserTextMessageEventArgs(Context.ConnectionId,message,UserTextMessageEventArgs.MessageType.NormalChat));
            Debug.WriteLine("FIRING");
            await Clients.All.SendAsync("SendedResponse", message);
        }



    }
}


