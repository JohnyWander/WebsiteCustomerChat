using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class AdminChatHub : Hub
    {
        ChatModule _chatModule;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminChatHub(IHttpContextAccessor httpContextAccessor, ChatModule module)
        {
            _httpContextAccessor = httpContextAccessor;
            _chatModule = module;


        }

        private bool Authorized(string Token)
        {

            return false;

        }

        public async Task NewClient(string SyncData)
        {
            /*  //check if client is from reconnect - 
              string cookie=ChatEvents.CheckIfClientReconnected();
              if(cookie != null)
              {
                  Debug.WriteLine(cookie);
                  await Clients.All.SendAsync("reconClient");
              }
            */
            await Clients.All.SendAsync("NewClient", "new client");
        }

        public async Task SendText(string ID, string text)
        {
            Debug.WriteLine("Operator responded:" + text);
            await _chatModule.OperatorTextMessage(ID, text);
        }

        public async Task ChangeMyName(string newName)
        {
            await _chatModule.OperatorNameChange(Context.ConnectionId, newName);
        }

        public async Task<string> GetExistingWorkspace()
        {

            return await _chatModule.GetOperatorExsistingWorkspace();
        }

        public async Task<string> GetNotSavedChanges()
        {
            return "";
        }


        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            await _chatModule.OperatorConnected(connectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            string connectionId = Context.ConnectionId;
            await _chatModule.OperatorDisconnected(connectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
