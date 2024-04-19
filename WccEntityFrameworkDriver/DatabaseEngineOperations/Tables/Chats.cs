using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Tables
{
    public class Chats
    {
        [Key]
        public int ChatID { get; set; }

        public int StaffID { get; set; }

        public int MessegesFromClient { get; set; }

        public int MessegesFromStaff { get; set; }
        
        public List<string> ReceivedFilePaths { get; set; }
        public List<string> SendedFilePaths { get; set; }


        public DateTime TimeStarted { get; set; }

        public DateTime TimeEnded { get;set; }

    }
}
