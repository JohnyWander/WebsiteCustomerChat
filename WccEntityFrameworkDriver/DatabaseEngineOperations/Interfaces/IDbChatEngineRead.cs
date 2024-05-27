using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    public interface IDbChatEngineRead
    {   
        public Task<string> GetChatHistoryFromDb(string connectionIDfromCookie,string newId);

        public void Dispose();
        public ValueTask DisposeAsync();
    }
}
