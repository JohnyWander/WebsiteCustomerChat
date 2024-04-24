using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using WebsiteCustomerChatMVC.SignarR.Hubs;
using WebsiteCustomerChatMVC.SignarR.Hubs.Messages;
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;

namespace WebsiteCustomerChatMVC.SignarR
{
    public class ChatModule
    {

        private IHubContext<ChatHub> _hubContext;
        private IHubContext<AdminChatHub> _adminContext;

        

        private IDictionary<string, ConnectedClient> ChatClients = new Dictionary<string, ConnectedClient>();
        private IDictionary<string,ConnectedClient> AdminClients = new Dictionary<string, ConnectedClient>();

        private IDbChatEngine db = new MySQLengine();
        public ChatModule(IHubContext<ChatHub> hubContext,IHubContext<AdminChatHub> adminContext)
        {

            

            ConfigureClientToOperatorEvents();
            ConfigurOperatorToClientEvents();
            _hubContext = hubContext;
            _adminContext = adminContext;

            if(_adminContext is null)
            {
                throw new Exception("SHIT");
            }

            Debug.WriteLine("Module constructor finished");

        }

        private void ConfigureClientToOperatorEvents()
        {

            ChatEvents.UserConnectedEvent += (object sender, UserConnectedEventArgs e) =>
            {
                ChatClients.Add(e.ConnectionID, new ConnectedClient(e.ConnectionID));

                if (e.HasCookie && e.CookieConnId!="" && e.CookieConnId!=" ")
                {
                   string History = db.GetChatHistoryFromDb(e.CookieConnId, e.ConnectionID);
                    _hubContext.Clients.Client(e.ConnectionID).SendAsync("LoadHistory",History,e.ConnectionID);
                    _adminContext.Clients.Client(e.ConnectionID).SendAsync("NewClient", e.ConnectionID, "").Wait();
                    Debug.WriteLine("reconnected");
                }
                else
                {
                    _adminContext.Clients.All.SendAsync("NewClient", e.ConnectionID, "xsd").Wait() ;
                    Debug.WriteLine("FIRST CONN");
                }
            };

            ChatEvents.UserDisconnectedEvent += (object sender, UserConnectedEventArgs e) =>
            {
                db.PushChatHistoryToDb(e.ConnectionID, ChatClients[e.ConnectionID].ExportMessages(), ChatClients[e.ConnectionID].StartDate,DateTime.Now).Wait();
                Debug.WriteLine("Client went out");
            };




            ChatEvents.UserTextMessageEvent += (object sender, UserTextMessageEventArgs e) =>
            {
                ChatClients[e.ConnectionID].Messages.Add(new MessageBase(MessageBase.MessageType.text,MessageBase.MessageDirection.ToOperator, e.Text));
                _adminContext.Clients.All.SendAsync("NewMessage", e.ConnectionID, e.Text).Wait();

            };

        }

        private void ConfigurOperatorToClientEvents()
        {
            ChatEvents.OperatorTextMessageEvent += (object sender, UserTextMessageEventArgs e) =>
            {
                ChatClients[e.ConnectionID].Messages.Add(new MessageBase(MessageBase.MessageType.text, MessageBase.MessageDirection.ToClient, e.Text));
                _hubContext.Clients.Client(e.ConnectionID).SendAsync("ReceiveMessage", "Operator", e.Text);
            };

        }



    }
}
