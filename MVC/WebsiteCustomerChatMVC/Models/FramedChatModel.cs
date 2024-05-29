using WebsiteCustomerChatConfiguration;
namespace WebsiteCustomerChatMVC.Models
{
    public class FramedChatModel
    {
        internal string ListenOn;
        public FramedChatModel(string adress)
        {

            ListenOn = Config.ParsedConfiguration.FirstOrDefault(x => x.Name == "ListenOn").Value ?? adress;
        }

    }
}
