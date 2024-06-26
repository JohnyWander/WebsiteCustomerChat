﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Tables;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.DataSets
{
    public class LoggedUserContext
    {
        string Username;
    

        DateTime CreationData;
        DateTime LastLogin;

        public readonly bool NoUserContext;

        public LoggedUserContext(string username)
        {
            Username = username;
           
          
        }

        private LoggedUserContext()
        {
            NoUserContext = true;
        }

        public static LoggedUserContext CreateFailedUserContext()
        {
            return new LoggedUserContext();
        }

    }
}
