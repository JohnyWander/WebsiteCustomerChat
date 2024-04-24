using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces
{
    public interface IDbChatEngine
    {

        public Task PushChatHistoryToDb(string connectionID, string ChatData, DateTime DateStarted, DateTime DateEnded);

        public string GetChatHistoryFromDb(string connectionIDfromCookie,string newId);
    }
}
