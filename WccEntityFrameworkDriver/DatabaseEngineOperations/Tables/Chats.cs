using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Metadata;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Helpers;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Tables;
using WccEntityFrameworkDriver.DatabaseEngineOperations.DataSets;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Tables
{
    public class Chats
    {
        [Key]
        public int ChatID { get; set; }

        public ICollection<Users> UsersResp { get; set; }

        public byte[] ConversationBlob { get; set; }
        
        public string ReceivedFilePaths { get; set; }
        public string SendedFilePaths { get; set; }


        public DateTime TimeStarted { get; set; }

        public DateTime TimeEnded { get;set; }

        

    }
}
