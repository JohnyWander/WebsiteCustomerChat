using MySql.Data.MySqlClient;
using WebsiteCustomerChatConfiguration;
namespace WebsiteCustomerChatMVC.DatabaseNoEF.MySql
{
    public class MysqlBase :IDisposable,IAsyncDisposable
    {
        private protected string _server;
        private protected string _dbport;
        private protected string _dbname;
        private protected string _dbuser;
        private string _dbpassword;

        private protected MySqlConnection _connection;

        
        public MysqlBase()
        {
            
                _server = Config.GetConfigValue("DatabaseHost");
                _dbport = Config.GetConfigValue("DatabasePort");
                _dbname = Config.GetConfigValue("DatabaseName");
                _dbuser = Config.GetConfigValue("DatabaseUser");
                _dbpassword = Config.GetConfigValue("DatabasePassword");
                _connection = new MySqlConnection($"server={_server};port={_dbport};user={_dbuser};password={_dbpassword};database={_dbname}");
                _connection.Open();
                //var command = _connection.CreateCommand();
               /// command.CommandText = "SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;";
               // command.ExecuteNonQuery(); ;

                Console.WriteLine("CONNECTED MYSQL DB");

         
        }

       

        public MysqlBase(string server,string port,string dbname, string dbuser, string dbpassword)
        {
        
                _server = server;
            _dbport = port;
            _dbname = dbname;
            _dbuser = dbuser;
            _dbpassword = dbpassword;

           

            _connection = new MySqlConnection($"server={_server};port={_dbport};user={_dbuser};password={_dbpassword}");
            Console.WriteLine($"server={_server};port={_dbport};user={_dbuser};password={_dbpassword};database={_dbname}");
            _connection.Open();
        ////    var command = _connection.CreateCommand();
          //  command.CommandText = "SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;";
          //  command.ExecuteNonQuery(); ;

            Console.WriteLine("CONNECTED MYSQL DB");

       
        }

        public async void Dispose()
        {
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
            GC.SuppressFinalize(this);
        }



    }
}
