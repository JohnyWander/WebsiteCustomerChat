namespace WebsiteCustomerChatMVC.SignarR
{
    public class UserConnectedEventArgs : EventArgs
    {
        public string ConnectionID { get; }

        public string CookieConnId { get; }

        public bool HasCookie { get; } = false;

        

        public UserConnectedEventArgs(string Connectionid)
        {
            this.ConnectionID = Connectionid;
            
            
        }

        public UserConnectedEventArgs(string ConnectionId,string CookieConnId,bool hasCookie)
        {
            this.ConnectionID = ConnectionId;
            this.CookieConnId = CookieConnId;
            this.HasCookie = hasCookie;
        }

    }

    public class UserTextMessageEventArgs : EventArgs
    {
        public enum MessageType
        {
            InitialForm,
            NormalChat
        }
        public MessageType type;
        public string ConnectionID;
        public string Text;
        public UserTextMessageEventArgs(string Connectionid,string text,MessageType type)
        {
            this.ConnectionID = Connectionid;
            this.Text = text;
            this.type = type;
        }
    }

    public static class ChatEvents
    {
        internal static event EventHandler<UserConnectedEventArgs> UserConnectedEvent; // Client to operators
        internal static event EventHandler<UserConnectedEventArgs> UserConnectedWithCookie;
        internal static event EventHandler<UserTextMessageEventArgs> UserTextMessageEvent; // Client to operators
        internal static event EventHandler<UserConnectedEventArgs> UserDisconnectedEvent;
        



        internal static event EventHandler<UserTextMessageEventArgs> OperatorTextMessageEvent;

        internal static void FireDisconnected(object sender,UserConnectedEventArgs e)
        {
            UserDisconnectedEvent?.Invoke(sender, e);
        }

        internal static void FireConnected(object sender,UserConnectedEventArgs e)
        {
          
            
                UserConnectedEvent?.Invoke(sender, e);
           
        }

        internal static void FireTextToOperator(object sender, UserTextMessageEventArgs e)
        {
            UserTextMessageEvent?.Invoke(sender, e);
        }

        internal static void FireTextToClient(object sender,UserTextMessageEventArgs e)
        {
           OperatorTextMessageEvent?.Invoke(sender, e);
        }


    }
}
