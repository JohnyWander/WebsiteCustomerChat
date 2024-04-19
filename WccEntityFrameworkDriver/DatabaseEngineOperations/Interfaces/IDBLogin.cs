using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WccEntityFrameworkDriver.DatabaseEngineOperations.DataSets;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    public interface IDBLogin
    {
        public Task<LoggedUserContext> UserLogin(string Username,string Password);


    }
}
