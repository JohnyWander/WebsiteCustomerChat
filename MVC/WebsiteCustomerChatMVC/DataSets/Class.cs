namespace WebsiteCustomerChatMVC.DataSets
{
    public class LoggedUserContext
    {
        string Username;


        DateTime CreationData;
        DateTime LastLogin;

        public readonly bool NoUserContext;

        public LoggedUserContext(string username)
        {
            Username = username;


        }

        private LoggedUserContext()
        {
            NoUserContext = true;
        }

        public static LoggedUserContext CreateFailedUserContext()
        {
            return new LoggedUserContext();
        }

    }

}
