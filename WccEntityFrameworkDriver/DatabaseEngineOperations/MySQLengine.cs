using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public class MySQLengine : DatabaseEngine,IDbInstallation
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

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseMySQL($"server={_server};port={_dbport};database={_dbname};user={_dbuser};password={_dbpassword}");

        public async Task CheckForDbOrCreate()
        {
            await this.Database.EnsureCreatedAsync();
        }

    }
}
