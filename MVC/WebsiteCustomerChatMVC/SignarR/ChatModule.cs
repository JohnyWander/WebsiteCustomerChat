using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Threading.Tasks;
using WebsiteCustomerChatMVC.SignarR.Hubs;
using WebsiteCustomerChatMVC.SignarR.Hubs.Messages;

using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using WebsiteCustomerChatMVC.DatabaseNoEF.MySql;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace WebsiteCustomerChatMVC.SignarR
{
    public class ChatModule
    {
        
        
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IHubContext<AdminChatHub> _adminContext;

       

        private ConcurrentDictionary<string, ConnectedClient> ChatClients = new ConcurrentDictionary<string, ConnectedClient>();
        private ConcurrentDictionary<string, ConnectedClient> AdminClients = new ConcurrentDictionary<string, ConnectedClient>();


        public  string GenerateRandomToken(int length = 32)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[length];
                rng.GetBytes(byteArray);
                return Regex.Replace(Convert.ToBase64String(byteArray), "[^a-zA-Z0-9]", "");
                
            }
        }

        public async Task<string> GetClientToken(string AccessToken)
        {
            await Task.Delay(3000);
            return this.ChatClients[AccessToken].AccessToken;
        }


        SemaphoreSlim sem = new SemaphoreSlim(1);
        internal MySqlChatEngine dbnoef;
        public ChatModule(IHubContext<ChatHub> ch,IHubContext<AdminChatHub> ach)
        {
            _hubContext = ch;
            _adminContext = ach;
            this.dbnoef = new MySqlChatEngine();
        }

        internal void StartDBConnection()
        {
            dbnoef = new MySqlChatEngine();
        }

        internal void StopDbconnection()
        {
            dbnoef.Dispose();
        }

        internal async Task StopDbConnectionAsync()
        {
            await dbnoef.DisposeAsync();
        }

        internal async Task RecreateConnectionAsync()
        {
            if(dbnoef is not null) { 
            await dbnoef.DisposeAsync();
            }
            dbnoef = new MySqlChatEngine();
        }

    
     
        internal async Task UserConnected(string connectionID,string IP)
        {
            await UserConnected(connectionID, "", false, IP);
        }

        internal async Task UserConnected(string connectionID)
        {
            await UserConnected(connectionID, "", false, "");
        }

        internal async Task UserConnected(string connectionID, string cookieToken, bool HasCookie,string IP)
        {
           

            if (HasCookie && cookieToken != "" && cookieToken != " ")
            {
                // ConnectedClient ClientFromBefore = ChatClients[cookieToken];         
                ConnectedClient ClientFromBefore;
                bool exists = ChatClients.TryGetValue(cookieToken, out ClientFromBefore);
                  
                if(!exists) { 
                    ClientFromBefore = new ConnectedClient(connectionID, cookieToken, IP, true);
                    ChatClients.TryAdd(cookieToken,ClientFromBefore);
                }


                ClientFromBefore.IsFromReconnect = true;
                ClientFromBefore.IP = IP;
                ClientFromBefore.ConnectionID = connectionID;
             
                await _adminContext.Clients.All.SendAsync("reconClient", cookieToken, connectionID);

                
                
                string hist = await GetChatHistory(cookieToken);
                Console.WriteLine("SENDING HIST");
                await _hubContext.Clients.Client(connectionID).SendAsync("LoadHistory", hist, connectionID);
                
            }
            else
            {
                ConnectedClient client = new ConnectedClient(connectionID,GenerateRandomToken(),IP);
                
                ChatClients.TryAdd(client.AccessToken, client);
                
                await _adminContext.Clients.All.SendAsync("NewClient", client.AccessToken, "xsd");
                await _hubContext.Clients.Client(connectionID).SendAsync("SetCookie", $"{client.AccessToken}");
            }
        }

        internal async Task UserDisconnected(string connectionID)
        {
            Console.WriteLine("DISC");
            ConnectedClient client=null;
            
            client = ChatClients.FirstOrDefault(x => x.Value.ConnectionID == connectionID).Value;            
            await this.dbnoef.PushChatHistoryToDb(connectionID, client);
            
            
        }

        internal async Task<string> GetChatHistory(string AccessToken)
        {
            ConnectedClient client=null;

                client = ChatClients[AccessToken];

            
            try
            {
                return await dbnoef.GetChatHistoryFromDb(client);
            }
            finally
            {
                
            }


        }

        internal async Task<string> GetNotSavedChanges()
        {
            return "";
        }


        internal async Task UserTextMessage(string Text, string ConnectionID)
        {
            ConnectedClient client = ChatClients.FirstOrDefault(x => x.Value.ConnectionID == ConnectionID).Value;
              //ConnectedClient client = ChatClients[Token]; ;
            client.Messages.Add(new MessageBase(MessageBase.MessageType.text, MessageBase.MessageDirection.ToOperator, Text));
           await _adminContext.Clients.All.SendAsync("NewMessage", client.AccessToken, $"ToOperator|text|{Text}");
        }

        internal async Task UserNameChange(string ConnectionID, string NewName)
        {
            ChatClients[ConnectionID].DescribeMeAs = NewName;
        }

        internal async Task<string> UserGetsOperatorName()
        {
            string name = AdminClients?.FirstOrDefault().Value.DescribeMeAs;
            if (name == null)
                return "None";
            else
            {
                return name;
            }
        }



        /// ///////////// OP

        internal async Task OperatorConnected(string ConnectionID)
        {
            AdminClients.TryAdd(ConnectionID, new ConnectedClient(ConnectionID));
        }

        internal async Task OperatorDisconnected(string ConnectionID) 
        {
          //  AdminClients.TryRemove(ConnectionID);
        }

        internal async Task OperatorNameChange(string ConnectionID, string NewName)
        {
            AdminClients[ConnectionID].DescribeMeAs = NewName;
        }

        internal async Task<string> GetOperatorExsistingWorkspace()
        {
            return await dbnoef.GetOperatorWorkspace();
        }

        internal async Task OperatorTextMessage(string Token,string Text)
        {
            //ChatClients[ConnectionID].Messages.Add(new MessageBase(MessageBase.MessageType.text, MessageBase.MessageDirection.ToClient, Text));
            ConnectedClient client = ChatClients[Token];

            client.Messages.Add(new MessageBase(MessageBase.MessageType.text, MessageBase.MessageDirection.ToClient, Text));
            await  _hubContext.Clients.Client(client.ConnectionID).SendAsync("ReceiveMessage", "Operator", Text);
        }

  


    }
}
