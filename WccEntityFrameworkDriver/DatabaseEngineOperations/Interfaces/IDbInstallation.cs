using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    public interface IDbInstallation
    {
        public Task CreateAdminAccount(string Username, string Password);
         public Task CheckForDbOrCreate();
         public void Dispose();
         public ValueTask DisposeAsync();
    }
}
