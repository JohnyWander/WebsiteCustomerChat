using Microsoft.AspNetCore.SignalR;
// SignalR Hub
using System.Diagnostics;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class CookieWaiter
    {
        public string ConnectionID;
        public string CookieToken;



        public CookieWaiter(string ConnectionID, string token)
        {
            this.ConnectionID = ConnectionID;
            this.CookieToken = token;
        }


    }


    public class ClientHeartMonitor
    {
        public string ConnectionID;

    }


    public class ChatHub : Hub
    {

        ChatModule _chatModule;

        List<CookieWaiter> _cookies = new List<CookieWaiter>();



        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChatHub(IHttpContextAccessor httpContextAccessor, ChatModule module)
        {

            _httpContextAccessor = httpContextAccessor;
            _chatModule = module;

        }





        public override async Task OnConnectedAsync()
        {


            string connectionId = Context.ConnectionId;
            var cookieValue = _httpContextAccessor.HttpContext.Request.Query["c"].ToString();


            if (cookieValue != null && cookieValue != "")
            {
                Console.WriteLine("GOT TOKEN :" + cookieValue);
                _cookies.Add(new CookieWaiter(Context.ConnectionId, cookieValue));
                await _chatModule.UserConnected(connectionId, cookieValue, true, Context.GetHttpContext().Connection.RemoteIpAddress.ToString());


            }
            else
            {
                Console.WriteLine("NO TOKEN");
                await _chatModule.UserConnected(connectionId, Context.GetHttpContext().Connection.RemoteIpAddress.ToString());
            }



            await base.OnConnectedAsync();
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string connectionId = Context.ConnectionId;

            //await _chatModule.UserDisconnected(connectionId);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task NotifyRefresh()
        {
            Console.WriteLine("REFRESHED");
            string connectionId = Context.ConnectionId;
            await _chatModule.UserDisconnected(connectionId);

        }







        public async Task SendMessage(string message)
        {
            await _chatModule.UserTextMessage(message, Context.ConnectionId);

            Debug.WriteLine("FIRING");
            await Clients.Client(Context.ConnectionId).SendAsync("SendedResponse", message);
        }

        public async Task<string> getOperatorName()
        {

            Debug.WriteLine("GET OP");
            //await Clients.Client(Context.ConnectionId).SendAsync("",ChatEvents.UserGetsOperatorName());
            return await _chatModule.UserGetsOperatorName();
        }

        // public async Task CheckOnline()



    }
}


