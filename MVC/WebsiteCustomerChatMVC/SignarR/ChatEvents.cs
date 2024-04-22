namespace WebsiteCustomerChatMVC.SignarR
{
    public class UserConnectedEventArgs : EventArgs
    {
        public string ConnectionID { get; }
        public UserConnectedEventArgs(string Connectionid)
        {
            this.ConnectionID = Connectionid;
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
        internal static event EventHandler<UserConnectedEventArgs> UserConnectedEvent;
        internal static event EventHandler<UserTextMessageEventArgs> UserTextMessageEvent;

        internal static void FireConnected(object sender,UserConnectedEventArgs e)
        {
            UserConnectedEvent?.Invoke(sender, e);
        }

        internal static void FireText(object sender, UserTextMessageEventArgs e)
        {
            UserTextMessageEvent?.Invoke(sender, e);
        }



    }
}
