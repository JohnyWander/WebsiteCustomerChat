using WebsiteCustomerChatMVC.SignarR.Hubs.Messages;
using Microsoft.AspNetCore.SignalR;
namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class ConnectedClient
    {
        internal readonly string ConnectionID;

        internal readonly string DataLocation;

        internal List<MessageBase> Messages = new List<MessageBase>();

        


        public ConnectedClient(string connectionID)
        {
            this.ConnectionID = connectionID;
        }
    }
}
