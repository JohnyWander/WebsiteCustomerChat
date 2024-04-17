using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Tables;


namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public abstract class DatabaseEngine : DbContext, IDbInstallation,IDBcheckOnInit
    {

        //public DbSet<Blog> Blogs { get; set; }
        //public DbSet<Post> Postss { get; set; }

        public DbSet<Users> users { get; set; }
        public DbSet<Permissions> permissions { get; set; }
        public DbSet<Chats> chats { get; set; }




        public DatabaseEngine()
        {

            

        }

        

        protected abstract override void OnConfiguring(DbContextOptionsBuilder options);


        public Task CheckForDbOrCreate()
        {
            this.Database.EnsureCreated();
            return Task.CompletedTask;
        }

        private async Task AddDefaultPermissions()
        {
            this.permissions.AddRangeAsync(
                new Permissions { PermissionName = "ALL" },
                new Permissions { PermissionName ="ChatWithClient"}


                ); ;
           
            
        }

        private async Task AddDefaultRoles()
        {

        }


        public bool DatabaseExists()
        {
            return this.Database.CanConnect();
            
        }

       // public Task CreateUser(string username,string password,)

        



      

    }
}
