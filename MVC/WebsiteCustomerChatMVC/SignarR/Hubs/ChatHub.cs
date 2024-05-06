using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI;
// SignalR Hub
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Diagnostics;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{


    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public override async Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies["chatCookie"];

            if (cookieValue != null)
            {
                ChatEvents.FireConnected(this, new UserConnectedEventArgs(connectionId, cookieValue, true));
            }
            else
            {
                ChatEvents.FireConnected(this, new UserConnectedEventArgs(connectionId));
            }







            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;
            ChatEvents.FireDisconnected(this, new UserConnectedEventArgs(connectionId));
            await base.OnDisconnectedAsync(exception);
        }



        public async Task SendMessage(string message)
        {
            ChatEvents.FireTextToOperator(this, new UserTextMessageEventArgs(Context.ConnectionId, message, UserTextMessageEventArgs.MessageType.NormalChat));
            Debug.WriteLine("FIRING");
            await Clients.All.SendAsync("SendedResponse", message);
        }

        public string getOperatorName()
        {
            //await Clients.Client(Context.ConnectionId).SendAsync("",ChatEvents.UserGetsOperatorName());
            return ChatEvents.UserGetsOperatorName();
        }

        // public async Task CheckOnline()



    } 
}


