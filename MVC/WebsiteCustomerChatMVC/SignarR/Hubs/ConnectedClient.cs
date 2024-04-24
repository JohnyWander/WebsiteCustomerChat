using WebsiteCustomerChatMVC.SignarR.Hubs.Messages;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Security.Permissions;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class ConnectedClient
    {
        internal readonly string ConnectionID;

        internal readonly string DataLocation;

        internal List<MessageBase> Messages = new List<MessageBase>();

        internal DateTime StartDate = DateTime.Now;


        public ConnectedClient(string connectionID)
        {
            this.ConnectionID = connectionID;
        }

        public string ExportMessages()
        {
            StringBuilder b = new StringBuilder();

            Messages.ForEach(m =>
            {
                string direction = m.direction.ToString();
                string Type="";
                string msg="";
                if(m.messageType== MessageBase.MessageType.text)
                {
                    Type = "text";
                    msg = m.MessageData;
                }
                else if(m.messageType == MessageBase.MessageType.media)
                {
                    Type = "media";
                    msg = m.MediaLocalPath;
                }
                b.AppendLine($"{direction}|{Type}|{msg}");
            });
            return b.ToString();
        }
    }
}
