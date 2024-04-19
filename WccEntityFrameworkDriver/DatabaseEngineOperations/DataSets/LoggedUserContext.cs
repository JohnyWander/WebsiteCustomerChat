using System;
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
        string FirstName;
        string LastName;

        DateTime CreationData;
        DateTime LastLogin;

        public readonly bool NoUserContext;

        public LoggedUserContext(string username, string firstName, string lastName, DateTime creationData, DateTime lastLogin,Roles[] roles,Permissions[] permissions)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            CreationData = creationData;
            LastLogin = lastLogin;
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
