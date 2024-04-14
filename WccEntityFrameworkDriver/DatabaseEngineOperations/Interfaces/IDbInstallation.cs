using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    public interface IDbInstallation
    {

         public Task CheckForDbOrCreate();
    }
}
