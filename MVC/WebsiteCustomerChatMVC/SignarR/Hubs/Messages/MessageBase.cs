namespace WebsiteCustomerChatMVC.SignarR.Hubs.Messages
{
    internal class MessageBase
    {
        internal enum MessageType
        {
            text,
            media
        }


        internal enum MessageDirection
        {
            ToOperator,
            ToClient
        }

        internal MessageType messageType;
        internal MessageDirection direction;


        internal string MessageData;
        internal string MediaLocalPath;


        public MessageBase(MessageType messageType, MessageDirection messageDirection, string message)
        {
            this.messageType = messageType;
            this.direction = messageDirection;

            if (messageType == MessageType.text)
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
