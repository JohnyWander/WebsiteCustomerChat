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

    public class UserNameChangeEventArgs : EventArgs
    {
        public string ConnectionID { get; }
        public string NewName { get; }

        public UserNameChangeEventArgs(string ConnectionId, string NewName)
        {
            this.ConnectionID = ConnectionId;
            this.NewName = NewName;
        }
    }


    public static class ChatEvents
    {
        internal static event EventHandler<UserConnectedEventArgs> UserConnectedEvent; // Client to operators
        internal static event EventHandler<UserConnectedEventArgs> UserConnectedWithCookie;
        internal static event EventHandler<UserTextMessageEventArgs> UserTextMessageEvent; // Client to operators
        internal static event EventHandler<UserConnectedEventArgs> UserDisconnectedEvent;
        internal static event EventHandler<UserNameChangeEventArgs> UserNameChangeEvent;

        // internal static event EventHandler<UserTextMessageEventArgs> ;


        internal static Func<string> UserGetsOperatorName;

        internal static event EventHandler<UserConnectedEventArgs> OperatorconnectedEvent;
        internal static event EventHandler<UserConnectedEventArgs> OperatorDisconnectedEvent;
        internal static event EventHandler<UserTextMessageEventArgs> OperatorTextMessageEvent;
        internal static event EventHandler<UserNameChangeEventArgs> OperatorNameChangeEvent;

     
        internal static void FireUserNameChangeEvent(object sender,UserNameChangeEventArgs e)
        {
            UserNameChangeEvent?.Invoke(sender, e);
        }

        internal static void FireOperatorNameChangeEvent(object sender, UserNameChangeEventArgs e)
        {
            OperatorNameChangeEvent?.Invoke(sender, e);
        }

        internal static void FireOperatorDisconnected(object sender,UserConnectedEventArgs e)
        {
            OperatorDisconnectedEvent?.Invoke(sender,e);
        }
        
        internal static void FireOperatorConnected(object sender, UserConnectedEventArgs e)
        {
            OperatorconnectedEvent?.Invoke(sender, e);
        }

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
