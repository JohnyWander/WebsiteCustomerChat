using System.Diagnostics;
using System.Security.Cryptography;

namespace WebsiteCustomerChatMVC.Models
{
    public class InstallationViewModel
    {
        
        private string ?AdminUsername;
        private string ?AdminPassword;
        private string ?AdminConfirmPassword;

        

        private string? dbengine;
        private string? sqlitedbFile;

        private string? server;
        private string? port;
        private string? dbuser;
        private string? dbuserpassword;

        public InstallationViewModel(IFormCollection form)
        {
            AdminUsername = form["username"];
            AdminPassword = form["password"];
            AdminConfirmPassword = form["confirmPassword"];
            dbengine = form["dbengine"];
            
            if(dbengine == "sqlite")
            {

            }
            else if(dbengine == "mysql")
            {

            }


        
        }
    }
}
