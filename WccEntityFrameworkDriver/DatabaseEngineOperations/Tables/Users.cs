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
    public class Users
    {
        [Key]
        public int Id { get; set; }

        public byte[] p_hash { get; set; }
       // public byte[] p_salt { get; set; }


        public string username { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Roles> UserRole { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastLogin { get; set; }
        


    }
}
