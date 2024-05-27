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
using System.Reflection.Emit;
using System.Web;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq.Expressions;

namespace WccEntityFrameworkDriver.DatabaseEngineOperations
{
    public abstract class DatabaseEngine : DbContext, IDbInstallation,IDBcheckOnInit,IDBLogin,IDbChatEngineRead,IDbChatEngineWrite
    {

        //public DbSet<Blog> Blogs { get; set; }
        //public DbSet<Post> Postss { get; set; }

        public DbSet<Users> users { get; set; }
        public DbSet<Permissions> permissions { get; set; }
        public DbSet<Chats> chats { get; set; }

        public DbSet<Roles> roles { get; set; }

        SemaphoreSlim dbChangesSync = new SemaphoreSlim(1);
        public DatabaseEngine()
        {
           
            
            
        }

        public async Task RecreateConnection()
        {
            
            await this.Database.CloseConnectionAsync();
            await this.Database.OpenConnectionAsync();
        }

        protected abstract override void OnConfiguring(DbContextOptionsBuilder options);

        private void ConfigureUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasIndex(x => x.LastName).IsUnique(false);
            modelBuilder.Entity<Users>().Property(x => x.LastName).IsRequired(false);
            modelBuilder.Entity<Users>().HasIndex(x => x.username).IsUnique();
            modelBuilder.Entity<Users>().Property(e => e.FirstName).IsRequired(false);

            modelBuilder.Entity<Users>()
            .HasIndex(e => e.FirstName)
            .IsUnique(false);
        }

        private void ConfigureChats(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chats>().HasIndex(x => x.ReceivedFilePaths).IsUnique(false);
            modelBuilder.Entity<Chats>().Property(x => x.ReceivedFilePaths).IsRequired(false);
            modelBuilder.Entity<Chats>().HasIndex(x => x.SendedFilePaths).IsUnique(false);
            modelBuilder.Entity<Chats>().Property(e => e.SendedFilePaths).IsRequired(false);
        }


        private void ConfigureRelations(ModelBuilder modelBuilder)
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

            modelBuilder.Entity<Users>()
                .HasMany(u => u.Chats)
                .WithMany(r => r.UsersResp)
                .UsingEntity<Dictionary<string, object>>(
                "UserChats",
                ur => ur.HasOne<Chats>().WithMany().HasForeignKey("ChatId"),
                ur => ur.HasOne<Users>().WithMany().HasForeignKey("UserId")

                );

            modelBuilder.Entity<Chats>()
               .HasMany(r => r.UsersResp)
               .WithMany(p => p.Chats)
               .UsingEntity<Dictionary<string, object>>(
               "StaffResponsibleForChat",
               r => r.HasOne<Users>().WithMany().HasForeignKey("ChatId"),
               r => r.HasOne<Chats>().WithMany().HasForeignKey("UserId")


               );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  modelBuilder.Entity<Chats>().HasIndex(x=>x.)           
            ConfigureUsers(modelBuilder);
            ConfigureChats(modelBuilder);
            ConfigureRelations(modelBuilder);        
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
            byte[] salt;
            CryptoHelper crypt = new CryptoHelper();
            await users.AddAsync(new Users
            {

                username = Username,
                p_hash = crypt.BcryptPasswordHash(Password, out salt),
                p_salt = salt,
                UserRole = new List<Roles> { roles.FirstOrDefault(x => x.Name == "Administrator") },
                CreationDate = DateTime.Now
            }); ;


            await SaveChangesAsync();
        }

        public async Task<LoggedUserContext> UserLogin(string Username,string Password)
        {
            var user = await users.Include(i=> i.UserRole)
                .ThenInclude(ur=>ur.RolePermissions)               
                .FirstOrDefaultAsync(u => u.username == Username);

            CryptoHelper crypt = new CryptoHelper();

            if (user is null)
            {
                return LoggedUserContext.CreateFailedUserContext();
            }
            else
            {
                byte[] p_hash = user.p_hash;
                byte[] p_salt = user.p_salt;

                bool PasswordOk = crypt.BcryptVerifyPassword(Password, p_hash, p_salt);
                if (!PasswordOk)
                {
                    return LoggedUserContext.CreateFailedUserContext();
                }
                else
                {
                    ICollection<Roles> UserRole = user.UserRole;

                    List<Permissions> UserPermissions = new List<Permissions>();

                    UserRole.ToList().ForEach(ur =>
                    {
                        ur.RolePermissions.ToList().ForEach(perm =>
                        {
                            UserPermissions.Add(perm);
                        });

                    });
                    

                    return new LoggedUserContext(user.username,
                        user.FirstName,
                        user.LastName,
                        user.CreationDate,
                        user.LastLogin,
                        UserRole.ToArray(),
                        UserPermissions.ToArray()
                        ) ;

                }
                
            }

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

        public async Task PushChatHistoryToDb(string connectionID, string ChatData,DateTime DateStarted,DateTime DateEnded)
        {
            
            

            //await dbChangesSync.WaitAsync();
            this.ChangeTracker.AutoDetectChangesEnabled = false;
            byte[] chatblob = Encoding.UTF8.GetBytes(ChatData);                      
            Chats chat = await this.chats.FirstOrDefaultAsync(x => x.ConnectionId == connectionID);

            if (chat != null)
            {
                chatblob = chatblob.Concat(chat.ConversationBlob).ToArray();


                await this.Database.ExecuteSqlRawAsync("UPDATE chats set ConversationBlob = {0} where connectionId = {1}",chatblob, connectionID );
            }
            else
            {
                await this.Database.ExecuteSqlRawAsync("INSERT INTO chats (connectionId,ConversationBlob, TimeStarted, TimeEnded) VALUES ({0},{1},{2},{3})", connectionID,chatblob,DateStarted,DateEnded );

            }


            /*
            if (chat != null)
            {
                chatblob = chatblob.Concat(chat.ConversationBlob).ToArray();
                chat.ConversationBlob = chatblob;
                this.Entry(chat).State = EntityState.Modified;
            }
            else
            {
                
             
                
               

            }
            
            await this.SaveChangesAsync();
            dbChangesSync.Release();
   */
        }


        public async Task<string> GetChatHistoryFromDb(string connectionIDfromCookie, string newId)
        {
            try
            {
                Console.WriteLine(connectionIDfromCookie + "BRUHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
                
                string hist = await MakeChanges(connectionIDfromCookie, newId);
              
                return hist;
            }
            catch
            {
                // try 3 times more and wait every time 2 secs
                for (int i = 0; i <= 3; i++)
                {
                    try
                    {
                        await Task.Delay(5000);
                        string hist = await MakeChanges(connectionIDfromCookie, newId);

                        return hist;
                    }
                    catch
                    {
                        continue;
                    }
                }
                throw;
            }
           
 
        }

        public async Task<string> MakeChanges(string connectionIDfromCookie,string newId)
        {
            
            
            Chats chat =await chats.FirstOrDefaultAsync(x => x.ConnectionId == connectionIDfromCookie);
            Debug.WriteLine(connectionIDfromCookie + " ++" + newId);
            byte[] data = chat.ConversationBlob;
            Console.WriteLine("DATA BLOB LENGTH:" + data.Length);
            

            await this.Database.ExecuteSqlRawAsync("UPDATE chats set connectionId = {0} where connectionId = {1}",newId, connectionIDfromCookie);

            //await this.SaveChangesAsync();

            return Encoding.UTF8.GetString(data);
        }






    }
}
