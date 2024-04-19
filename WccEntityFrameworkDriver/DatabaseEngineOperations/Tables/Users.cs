using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Paddings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations.Tables
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] p_hash { get; set; }

        [Required]
        public byte[] p_salt { get; set; }

        [Required]
       
        public string username { get; set; }

        [AllowNull]
        public string FirstName { get; set; }

        [AllowNull]
        public string LastName { get; set; }

        public ICollection<Roles> UserRole { get; set; }

        [AllowNull]
        public DateTime CreationDate { get; set; }

        [AllowNull]
        public DateTime LastLogin { get; set; }
        
        public ICollection<Chats> Chats { get; set; }

    }
}
