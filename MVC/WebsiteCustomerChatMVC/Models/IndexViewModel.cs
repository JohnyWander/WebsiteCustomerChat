using MySqlX.XDevAPI;
using WebsiteCustomerChatConfiguration;

namespace WebsiteCustomerChatMVC.Models
{
    public class IndexViewModel
    {
        public bool ConfigurationFileExists;
        public bool RedirectFromBadLogin;
      

        public IndexViewModel(bool BadLogin)
        {
            CheckForConfigFile();
            this.RedirectFromBadLogin = BadLogin;
        }
        

        private void CheckForConfigFile()
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
