using WebsiteCustomerChatConfiguration;
using WebsiteCustomerChatMVC.DatabaseNoEF.MySql;

namespace WebsiteCustomerChatMVC.Models
{
    public class InstallationViewModel
    {

        private string AdminUsername;
        private string AdminPassword;
        private string AdminConfirmPassword;



        private string dbengine;
        private string sqlitedbFile;

        private string server;
        private string port;
        private string dbname;
        private string dbuser;
        private string dbuserpassword;

        internal MysqlUserEngine MysqlUserEngine;
        internal MySqlInstallEngine MysqlInstallEngine;

        internal bool ConfigOK;
        internal bool DatabaseOK;
        internal bool AdminOK;


        public InstallationViewModel(IFormCollection form)
        {

            AdminUsername = form["username"];
            AdminPassword = form["password"];
            AdminConfirmPassword = form["confirmPassword"];
            dbengine = form["dbengine"];

            if (dbengine == "sqlite")
            {
                //   sqlitedbFile = form["sqlitedbFile"];
                //  DBengine = new SQLITEengine(sqlitedbFile);
                throw new NotImplementedException("sqlite coming soon"); // TODO: sqlite support
            }
            else if (dbengine == "mysql")
            {
                server = form["server"];
                port = form["port"];
                dbname = form["name"];
                dbuser = form["user"];
                dbuserpassword = form["dbuserpassword"];

                //  MysqlUserEngine = new MysqlBase(server, port, dbname, dbuser, dbuserpassword) as MysqlUserEngine;
                this.MysqlInstallEngine = new MySqlInstallEngine(server, port, dbname, dbuser, dbuserpassword);
            }
            /*

            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            PropertyInfo[] properties = type.GetProperties();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    string value = (string)field.GetValue(this);
                    Console.WriteLine($"{field.Name}: {value}");
                }
            }
            */
        }

        public async Task Setconfiguration()
        {

            await Config.SetConfigValue("DatabaseEngine", dbengine);
            if (this.dbengine == "mysql")
            {
                await Config.SetConfigValue("DatabaseName", this.dbname);
            }
            else
            {
                await Config.SetConfigValue("DatabaseName", this.sqlitedbFile);
            }
            await Config.SetConfigValue("DatabaseUser", this.dbuser);
            await Config.SetConfigValue("DatabasePassword", this.dbuserpassword);
            await Config.SetConfigValue("DatabaseHost", this.server);
            await Config.SetConfigValue("DatabasePort", this.port);


        }

        public async Task<string> TryParseConfig()
        {
            try
            {
                await Config.ParseConfig();
                this.ConfigOK = true;
                return "Success!";
            }
            catch (ConfigurationException ex)
            {
                return $"Parsing configuration failed with error - {ex.GetType().Name} - {ex.Message}";
            }

        }


        public async Task<string> EnsureOrCreate()
        {
            try
            {

                await this.MysqlInstallEngine.EnsureExistanceOrCreate(dbname);
                this.DatabaseOK = true;
                return "Success";
            }
            catch (Exception ex)
            {
                return $"DB creation failed with exception -{ex.GetType().Name} {ex.Message} ";
            }
        }

        public async Task<string> CreateAdminAccount()
        {
            try
            {
                MysqlUserEngine en = new MysqlUserEngine();
                await en.CreateAdminAccount(this.AdminUsername, this.AdminPassword);
                this.AdminOK = true;
                return "Success";
            }
            catch (Exception ex)
            {
                string mes = $"Error when creating Administrator account -{ex.GetType().Name} {ex.Message} {ex.InnerException.GetType().Name} {ex.InnerException.Message}";
                if (ex.InnerException.InnerException.Message != null)
                {
                    mes += " " + ex.InnerException.InnerException.Message;
                }
                return $"Error when creating Administrator account -{ex.GetType().Name} {ex.Message} {ex.InnerException.GetType().Name} {ex.InnerException.Message}";
            }

        }


    }
}
