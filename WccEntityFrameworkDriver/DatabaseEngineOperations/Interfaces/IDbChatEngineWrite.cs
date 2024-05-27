using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    public interface IDbChatEngineWrite
    {

        public Task PushChatHistoryToDb(string connectionID, string ChatData, DateTime DateStarted, DateTime DateEnded);

      

        public void Dispose();
        public ValueTask DisposeAsync();

    }
}
