using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces {

public interface IDBcheckOnInit
    {

        public bool DatabaseExists();
        public void Dispose();

        public Task CheckForDbOrCreate();



        public ValueTask DisposeAsync();
    }
}
