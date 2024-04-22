using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using WebsiteCustomerChatMVC.SignarR.Hubs;
using WebsiteCustomerChatMVC.SignarR.Hubs.Messages;

namespace WebsiteCustomerChatMVC.SignarR
{
    public class ChatModule
    {

        private IHubContext<ChatHub> _hubContext;
        private IHubContext<AdminChatHub> _adminContext;

        private ChatHub _chatHub;
        private AdminChatHub _adminChatHub;

        private IDictionary<string, ConnectedClient> ChatClients = new Dictionary<string, ConnectedClient>();
        private IDictionary<string,ConnectedClient> AdminClients = new Dictionary<string, ConnectedClient>();

        public ChatModule(IHubContext<ChatHub> hubContext,IHubContext<AdminChatHub> adminContext)
        {
            _hubContext = hubContext;
            _adminContext = adminContext;

            ChatEvents.UserConnectedEvent += (object sender, UserConnectedEventArgs e) =>
            {
                ChatClients.Add(e.ConnectionID, new ConnectedClient(e.ConnectionID));
                _adminContext.Clients.All.SendAsync("NewClient", e.ConnectionID, "").Wait();
            };

            ChatEvents.UserTextMessageEvent += (object sender, UserTextMessageEventArgs e) =>
            {
                ChatClients[e.ConnectionID].Messages.Add(new MessageBase(MessageBase.MessageType.text, e.Text));
                _adminContext.Clients.All.SendAsync("NewMessage", e.ConnectionID, e.Text).Wait();

            };
                     
        }

        
        

    }
}
