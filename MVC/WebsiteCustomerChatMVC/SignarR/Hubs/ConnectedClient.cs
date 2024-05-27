using WebsiteCustomerChatMVC.SignarR.Hubs.Messages;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Security.Permissions;

namespace WebsiteCustomerChatMVC.SignarR.Hubs
{
    public class ConnectedClient
    {
        internal  string ConnectionID;

        internal readonly string DataLocation;

        internal string DescribeMeAs;

        internal List<MessageBase> Messages = new List<MessageBase>();

        internal DateTime StartDate = DateTime.Now;


        internal bool IsFromReconnect = false;

        internal string AccessToken;
        internal string IP;

        internal bool LoadedFromDBAtStart;

       /*
        internal SemaphoreSlim dbsync = new SemaphoreSlim(0,1);

        internal TaskCompletionSource Synced;

        internal void RebootTCS()
        {
            Synced = new TaskCompletionSource();
        }
    
        */

        public ConnectedClient(string connectionID, string accessToken,string IP )
        {
            this.ConnectionID = connectionID;
            this.AccessToken = accessToken;
            this.IP = IP;
            
            AccessToken = accessToken;
        }

        public ConnectedClient(string connectionID,string Cookie,string IP,bool fromDbLoad)
        {
            this.ConnectionID =connectionID;
            this.AccessToken=Cookie;
            this.IP = IP;
            this.LoadedFromDBAtStart = fromDbLoad;
        }

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
