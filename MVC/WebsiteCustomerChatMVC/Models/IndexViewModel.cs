using WebsiteCustomerChatConfiguration;

namespace WebsiteCustomerChatMVC.Models
{
    public class IndexViewModel
    {
        public bool ConfigurationFileExists;

       public IndexViewModel()
       {
            if (File.Exists(Config.configFilename))
            {
                ConfigurationFileExists = true;
            }
            else
            {
                ConfigurationFileExists = false;
            }
       }


    }
}
