using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public class SQLITEengine : DatabaseEngine
    {
        private string _DBFileLocation;
        public SQLITEengine(string dbFileLocation)
        {

            _DBFileLocation = dbFileLocation;



        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite($"Data Source={_DBFileLocation}");



    }
}
