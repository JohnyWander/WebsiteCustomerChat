using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    internal interface IDBcheckOnInit
    {

        public bool DatabaseExists();
        public void Dispose();

        public ValueTask DisposeAsync();
    }
}
