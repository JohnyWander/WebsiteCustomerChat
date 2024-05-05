using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class AdminChatHub : Hub
    {
        private bool Authorized(string Token)
        {
            
            return false;

        }

        public async Task NewClient(string SyncData)
        {
           await Clients.All.SendAsync("NewClient", "new client");
        }

        public async Task SendText(string ID,string text)
        {
            Debug.WriteLine("Operator responded:"+text);
            ChatEvents.FireTextToClient(this, new UserTextMessageEventArgs(ID, text, UserTextMessageEventArgs.MessageType.NormalChat));
        }

        public async Task ChangeMyName(string newName)
        {
            ChatEvents.FireOperatorNameChangeEvent(this, new UserNameChangeEventArgs(Context.ConnectionId, newName));
        }

        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;

            ChatEvents.FireOperatorConnected(this, new UserConnectedEventArgs(connectionId));
  
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            
            string connectionId = Context.ConnectionId;
            ChatEvents.FireOperatorDisconnected(this,new UserConnectedEventArgs(connectionId));


            await base.OnDisconnectedAsync(exception);
        }
    }
}
