using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace WebsiteCustomerChatMVC.DatabaseNoEF.MySql
{
    public class MySqlInstallEngine : MysqlBase
    {

        public MySqlInstallEngine() : base()
        {

        }

        public MySqlInstallEngine(string server, string port, string dbname, string dbuser, string dbpassword) : base( server,port,dbname, dbuser, dbpassword) { }



        public async Task<string> EnsureExistanceOrCreate(string dbname)
        {
                MySqlCommand check = new MySqlCommand("", _connection);
            
                check.CommandText =
                @"SELECT SCHEMA_NAME
                FROM INFORMATION_SCHEMA.SCHEMATA
                WHERE SCHEMA_NAME = @db";
                check.Parameters.AddWithValue("@db", dbname);


                try
                {
                    var r = await check.ExecuteReaderAsync();
                if (!r.HasRows)
                {

                    await check.DisposeAsync();
                    r.DisposeAsync();


                    MySqlCommand createDB = new MySqlCommand($"CREATE DATABASE {dbname};use {dbname}", _connection);
                
                    
                    
                    await createDB.ExecuteNonQueryAsync();
                    await createDB.DisposeAsync();

                    await CreateUsersTable();
                    await CreateRolesTable();
                    await CreatePermissionsTable();
                    await CreateUserRolesTable();
                    await CreateRolePermissionsTable();
                    await CreateChatsTable();
                    await AddDefaultPermissions();
                    await AddDefaultRoles();                 
                    await AddDefaultPermsToRoles();

                    return "OK";
                }
                else
                {
                    await check.DisposeAsync();
                    r.DisposeAsync();

                    MySqlCommand createDB = new MySqlCommand($"use {dbname};", _connection);



                    await createDB.ExecuteNonQueryAsync();
                    await createDB.DisposeAsync();

                    await CreateUsersTable();
                    await CreateRolesTable();
                    await CreatePermissionsTable();
                    await CreateUserRolesTable();
                    await CreateRolePermissionsTable();
                    await CreateChatsTable();
                    await AddDefaultPermissions();
                    await AddDefaultRoles();
                    await AddDefaultPermsToRoles();

                    return "OK";
                }

                
                }catch(Exception ex)
                {
                    return $"ERROR!: {ex.GetType().Name} -  {ex.Message} - {ex.InnerException}"; 
                }

        }





        private async Task CreateUsersTable()
        {
            using (MySqlCommand createTable = new MySqlCommand("", _connection)) {
                createTable.CommandText =@"create table users(
	 Id int PRIMARY KEY AUTO_INCREMENT,
	 p_hash longblob,
	 p_salt longblob,
	 username varchar(255) UNIQUE,
	 FirstName varchar(255),
	 LastName varchar(255),
	 CreationDate datetime(6),
	 LastLogin datetime(6)

 );";
                await createTable.ExecuteNonQueryAsync();

            }
     
                
            
        }

        private async Task CreateRolesTable()
        {
            
           using(MySqlCommand createTable = new MySqlCommand("",_connection))
           {
                createTable.CommandText = 
                @"CREATE TABLE roles (
                    Id int PRIMARY KEY AUTO_INCREMENT,
                    Name VARCHAR(50) UNIQUE NOT NULL
                );
                ";
                await createTable.ExecuteNonQueryAsync();
           }

        }

        private async Task CreatePermissionsTable()
        {
            using(MySqlCommand createTable = new MySqlCommand("",_connection))
            {
                createTable.CommandText =
                @"CREATE TABLE permissions(
                    PermissionID int PRIMARY KEY AUTO_INCREMENT,
                    PermissionName VARCHAR(50) UNIQUE NOT NULL
                );
                ";
                await createTable.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateUserRolesTable()
        {
            using (MySqlCommand createTable = new MySqlCommand("", _connection))
            {
                createTable.CommandText =
                @"Create TABLE user_roles(
                    user_id INT,
                    role_id INT,
                    PRIMARY KEY(user_id,role_id),
                    FOREIGN KEY(user_id) REFERENCES users(Id) ON DELETE CASCADE,
                    FOREIGN KEY(role_id) REFERENCES roles(Id) ON DELETE CASCADE
                );
                ";

                await createTable.ExecuteNonQueryAsync();
            }




        }

        private async Task CreateRolePermissionsTable()
        {
            using(MySqlCommand createTable = new MySqlCommand("", _connection))
            {
                createTable.CommandText =
                @"Create Table role_permissions(
                    role_id INT,
                    permission_id INT,
                    PRIMARY KEY(role_id,permission_id),
                    FOREIGN KEY(role_id) REFERENCES roles(Id) ON DELETE CASCADE,
                    FOREIGN KEY(permission_id) REFERENCES permissions(PermissionId) ON DELETE CASCADE
                );
                ";
                await createTable.ExecuteNonQueryAsync();
            }

        }

        private async Task CreateChatsTable()
        {
            using (MySqlCommand createTable = new MySqlCommand("", _connection))
            {
                createTable.CommandText =
                    @"CREATE TABLE chats(
                        ChatID int primary key auto_increment,
                        access_token longtext not null,
                        client_endpoint longtext not null,
                        user_id int,
                        ConversationBlob longblob,
                        ReceivedFilePaths longtext,
                        SendedFilePaths longtext,
                        TimeStarted DATE,
                        TimeEnded DATE,                       
                        FOREIGN KEY (user_id) REFERENCES users(Id)

                    );";

                await createTable.ExecuteNonQueryAsync();

            }
        }



        private async Task AddDefaultPermissions()
        {
            using(MySqlCommand addPerms = new MySqlCommand("", _connection))
            {
                addPerms.CommandText = "INSERT INTO permissions(PermissionName) VALUES (\"ALL\")";
                await addPerms.ExecuteNonQueryAsync();
            }

        }

        private async Task AddDefaultRoles()
        {
           using(MySqlCommand addroles = new MySqlCommand("",_connection))
           {
                addroles.CommandText =
                @"INSERT INTO roles(Name) VALUES (""Admin"")";
                await addroles.ExecuteNonQueryAsync();              
           }
        }

        private async Task AddDefaultPermsToRoles()
        {
            using(MySqlCommand bind =  new MySqlCommand("",_connection))
            {
                bind.CommandText =
                @"INSERT INTO role_permissions(role_id,permission_id)
                  VALUES (
                (select Id from roles where name=""Admin""),
                (select permissionId from permissions where PermissionName=""ALL"")
                )";
                await bind.ExecuteNonQueryAsync();
            }
        }

    }
}
