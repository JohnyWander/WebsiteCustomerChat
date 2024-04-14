using System.Diagnostics;
using System.Security.Cryptography;
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;

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
        private string? dbname;
        private string? dbuser;
        private string? dbuserpassword;

        internal IDbInstallation DBengine;

        public InstallationViewModel(IFormCollection form)
        {
            AdminUsername = form["username"];
            AdminPassword = form["password"];
            AdminConfirmPassword = form["confirmPassword"];
            dbengine = form["dbengine"];
            
            if(dbengine == "sqlite")
            {
                sqlitedbFile = form["sqlitedbFile"];
            }
            else if(dbengine == "mysql")
            {
                server = form["server"];
                port = form["port"];
                dbname = form["name"];
                dbuser = form["user"];
                dbuserpassword = form["password"];

                DBengine = new MySQLengine(server,port,dbname,dbuser,dbuserpassword);
                DBengine.CheckForDbOrCreate().Wait();
            }

            /*
             * 
               Mysql host address&nbsp;<input type="text" class="mysqlinput" name="server" value="localhost" required><br>
            Mysql host port&nbsp;<input type="text" class="mysqlinput" name="port" value="3389" required><br>
            Database name&nbsp;<input type="text" class="mysqlinput" name="name" value="" required><br>
            Database user&nbsp;<input type="text" class="mysqlinput" name="user" value="" required><br>
            User password&nbsp;<input type="password" class="mysqlinput" name="dbuserpassword" value=""><br>
*/




        }
    }
}
