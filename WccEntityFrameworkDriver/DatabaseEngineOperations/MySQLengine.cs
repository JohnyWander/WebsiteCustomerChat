using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using WebsiteCustomerChatConfiguration;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public class MySQLengine : DatabaseEngine
    {
    
        private string _server;
        private string _dbport;
        private string _dbname;
        private string _dbuser;
        private string _dbpassword;

        public MySQLengine(string server,string dbport,string dbname,string dbuser,string dbpassword)
        {
            _server = server;
            _dbport = dbport;
            _dbname = dbname;
            _dbuser = dbuser;
            _dbpassword = dbpassword;
            Debug.WriteLine(_dbpassword);
        }

        public MySQLengine()
        {
            _server = Config.GetConfigValue("DatabaseHost");
            _dbport = Config.GetConfigValue("DatabasePort");
            _dbname = Config.GetConfigValue("DatabaseName");
            _dbuser = Config.GetConfigValue("DatabaseUser");
            _dbpassword = Config.GetConfigValue("DatabasePassword");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseMySQL($"server={_server};port={_dbport};database={_dbname};user={_dbuser};password={_dbpassword}");

        

    }
}
