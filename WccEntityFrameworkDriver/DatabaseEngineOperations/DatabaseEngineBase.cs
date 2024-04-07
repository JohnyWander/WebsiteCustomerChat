using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public abstract class DatabaseOperationsBase
    {
        public abstract void CreateDatabase();
        public abstract void DeleteDatabase();

        public abstract void BackupDatabase();

        public abstract void GetDatabaseVersion();

       
    }
}
