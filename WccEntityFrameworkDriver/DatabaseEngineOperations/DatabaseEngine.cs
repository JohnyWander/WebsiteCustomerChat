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

        public DbSet<Roles> roles { get; set; }


        public DatabaseEngine()
        {

            

        }

        

        protected abstract override void OnConfiguring(DbContextOptionsBuilder options);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasMany(u => u.UserRole)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                "UserRoles",
                ur => ur.HasOne<Roles>().WithMany().HasForeignKey("RoleId"),
                ur => ur.HasOne<Users>().WithMany().HasForeignKey("UserId")
            );


            modelBuilder.Entity<Roles>()
            .HasMany(r => r.RolePermissions)
            .WithMany(p => p.Roles)
            .UsingEntity<Dictionary<string, object>>(
                "RolePermissions",
                rp => rp.HasOne<Permissions>().WithMany().HasForeignKey("PermissionId"),
                rp => rp.HasOne<Roles>().WithMany().HasForeignKey("RoleId")
            );
        }

        public Task CheckForDbOrCreate()
        {
            this.Database.EnsureCreated();
            this.AddDefaultPermissions().Wait(); ;
            this.AddDefaultRoles().Wait();
            test();
            return Task.CompletedTask;
        }

        public async Task CreateAdminAccount(string Username,string Password)
        {
           
        }

        private async Task AddDefaultPermissions()
        {
            await this.permissions.AddRangeAsync(
                new Permissions { PermissionName = "ALL" },
                new Permissions { PermissionName ="ChatWithClient"},
                new Permissions { PermissionName ="ViewArchiveConversations"},
                new Permissions { PermissionName ="CreateUser"}
                ); ;

            await this.SaveChangesAsync();
        }

        private async Task AddDefaultRoles()
        {
            await this.roles.AddRangeAsync(
                new Roles { Name="Administrator",RolePermissions = new List<Permissions> ( permissions.Where(x=>x.PermissionName=="ALL")) } 
               // new Roles { }

                );
            await this.SaveChangesAsync();
        }


        public bool DatabaseExists()
        {
            return this.Database.CanConnect();
            
        }

        public void test()
        {
            Task.Delay(1000).Wait();
            var result = roles.Where(r => r.Name == "Administrator").Select(x => new { x.Name,x.RolePermissions }).ToList();

            result.ForEach(x => Debug.WriteLine(x.Name+string.Join(" ",x.RolePermissions.Select(p=>p.PermissionName))));

            
           
        }

       // public Task CreateUser(string username,string password,)

        



      

    }
}
