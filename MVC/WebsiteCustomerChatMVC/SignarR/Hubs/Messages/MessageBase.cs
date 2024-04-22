namespace WebsiteCustomerChatMVC.SignarR.Hubs.Messages
{
    internal class MessageBase
    {
        internal enum MessageType
        {
            text,
            media
        }

        internal MessageType messageType;


        internal string MessageData;
        internal string MediaLocalPath;


        public MessageBase(MessageType messageType, string message)
        {
            this.messageType = messageType;
           
            if(messageType == MessageType.text)
            {
                MessageData = message;
            }
            else
            {
                MediaLocalPath = message;
            }

        }



    }
}
