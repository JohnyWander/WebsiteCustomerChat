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
    
    public class Permissions
    {
        [Key]
        public int PermissionID { get; set; }

        public string PermissionName { get; set; }
    }
}
