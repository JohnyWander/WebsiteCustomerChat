using MySql.Data.MySqlClient;
using WebsiteCustomerChatMVC.DataSets;
using WebsiteCustomerChatMVC.Helpers;

namespace WebsiteCustomerChatMVC.DatabaseNoEF.MySql
{
    public class MysqlUserEngine : MysqlBase
    {
        public MysqlUserEngine() : base()
        {

        }


        public async Task CreateAdminAccount(string Username, string Password)
        {
            byte[] salt;
            CryptoHelper crypt = new CryptoHelper();

            using (MySqlCommand addadmin = new MySqlCommand("", _connection))
            {
                addadmin.CommandText =
                @"
                    INSERT INTO users(p_hash,p_salt,username) VALUES (@phash,@psalt,@username);
                ";
                addadmin.Parameters.AddWithValue("@phash", crypt.BcryptPasswordHash(Password, out salt));
                addadmin.Parameters.AddWithValue("@psalt", salt);
                addadmin.Parameters.AddWithValue("@username", Username);
                await addadmin.ExecuteNonQueryAsync();
            }

            using (MySqlCommand addRole = new MySqlCommand("", _connection))
            {
                addRole.CommandText =
                @"INSERT INTO user_roles(user_id,role_id) VALUES ((select Id from users where username=@username),(select Id from roles where Name=""Admin""));";
                addRole.Parameters.AddWithValue("@username", Username);
                await addRole.ExecuteNonQueryAsync();

            }


        }

        public async Task<LoggedUserContext> UserLogin(string Username, string Password)
        {
            using (MySqlCommand select = new MySqlCommand("", _connection))
            {
                /*          Id int PRIMARY KEY AUTO_INCREMENT,
               p_hash longblob,
               p_salt longblob,
               username varchar(255) UNIQUE,
               FirstName varchar(255),
               LastName varchar(255),
               CreationDate datetime(6),
               LastLogin datetime(6)
                */
                select.CommandText =
                @"select users.p_hash,users.p_salt,users.username,p.PermissionName
                from users join user_roles ur on ur.user_id = users.Id
                JOIN roles r ON ur.role_id = r.Id
                JOIN role_permissions rp ON r.Id = rp.role_id
                JOIN permissions p ON rp.permission_id = p.PermissionID
                WHERE users.username = @username
                ";
                select.Parameters.AddWithValue("@username", Username);
                var reader = select.ExecuteReader();



                CryptoHelper crypt = new CryptoHelper();

                if (!reader.HasRows)
                {
                    return LoggedUserContext.CreateFailedUserContext();
                }
                else
                {
                    await reader.ReadAsync();
                    byte[] p_hash = (byte[])reader["p_hash"];
                    byte[] p_salt = (byte[])reader["p_salt"];
                    string username = reader.GetString("username");

                    List<string> permissions = new List<string> { reader.GetString("PermissionName") };

                    while ((await reader.ReadAsync()))
                    {
                        permissions.Add(reader.GetString("PermissionName"));
                    }



                    bool PasswordOk = crypt.BcryptVerifyPassword(Password, p_hash, p_salt);
                    if (!PasswordOk)
                    {
                        return LoggedUserContext.CreateFailedUserContext();
                    }
                    else
                    {



                        return new LoggedUserContext(username); //TODO: more data for logged context such as permissions

                    }

                }
            }






        }



    }
}
