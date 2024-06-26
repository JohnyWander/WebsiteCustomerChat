﻿
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
    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Users> Users { get; set; }
        public ICollection<Permissions> RolePermissions { get; set; }
    }
}
