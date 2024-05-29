using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using WebsiteCustomerChatMVC.SignarR.Hubs;

namespace WebsiteCustomerChatMVC.DatabaseNoEF.MySql
{
    public class MySqlChatEngine : MysqlBase
    {

        public MySqlChatEngine() : base()
        {

        }


        public async Task PushChatHistoryToDb(string connectionID, ConnectedClient clientCallback)
        {


            byte[] chatblob = Encoding.UTF8.GetBytes(clientCallback.ExportMessages());
            MySqlCommand checkForChat = new MySqlCommand("select * from chats where access_token = @cid", _connection);
            checkForChat.Parameters.AddWithValue("@cid", clientCallback.AccessToken);
            var reader = await checkForChat.ExecuteReaderAsync();
            bool chat = reader.HasRows;



            if (await reader.ReadAsync())
            {
                Console.WriteLine("FOUND CHAT HISTORY");

                byte[] blobData = (byte[])reader["ConversationBlob"];
                await reader.DisposeAsync();

                chatblob = blobData.Concat(chatblob).ToArray();



                MySqlCommand update = new MySqlCommand("UPDATE chats set ConversationBlob = @data where access_token =@cid", _connection);
                update.Parameters.AddWithValue("@data", chatblob);
                update.Parameters.AddWithValue("@cid", clientCallback.AccessToken);

                Console.WriteLine("UPDATING CHAT:" + connectionID);
                int rowsAffected = await update.ExecuteNonQueryAsync();
                Console.WriteLine("FINISHED UPDATING CHAT:" + connectionID);

                await update.DisposeAsync();


                if (rowsAffected == 0)
                {
                    throw new Exception("Record should be modified and was not");
                }

            }
            else
            {
                await reader.DisposeAsync();

                MySqlCommand add = new MySqlCommand("INSERT INTO chats (access_token,client_endpoint,ConversationBlob, TimeStarted, TimeEnded) VALUES (@token,@end,@blob,@ds,@de)", _connection);
                add.Parameters.AddWithValue("@token", clientCallback.AccessToken);
                add.Parameters.AddWithValue("@end", clientCallback.IP);
                add.Parameters.AddWithValue("@blob", chatblob);
                add.Parameters.AddWithValue("@ds", clientCallback.StartDate);
                add.Parameters.AddWithValue("@de", DateTime.Now);

                Console.WriteLine("ADDING:" + connectionID);
                int affected = await add.ExecuteNonQueryAsync();
                Console.WriteLine("FINISHED ADDING");
                await add.DisposeAsync();

                if (affected == 0)
                {
                    {
                        throw new Exception("Record should be added but was not");
                    }

                }

            }

        }


        public async Task<string> GetChatHistoryFromDb(ConnectedClient clientCallback)
        {

            MySqlCommand checkForChat = new MySqlCommand("select * from chats where access_token = @cid", _connection);
            checkForChat.Parameters.AddWithValue("@cid", clientCallback.AccessToken);
            var reader = await checkForChat.ExecuteReaderAsync();


            if (reader.HasRows)
            {
                await reader.ReadAsync();
                byte[] blob = (byte[])reader["ConversationBlob"];
                await reader.DisposeAsync();
                await checkForChat.DisposeAsync();

                Console.WriteLine(Encoding.UTF8.GetString(blob));
                return Encoding.UTF8.GetString(blob);
            }
            else
            {
                Console.WriteLine("RESULT");
                await reader.DisposeAsync();
                await checkForChat.DisposeAsync();

                return "";

            }

        }

        public async Task<string> GetOperatorWorkspace()
        {
            MySqlCommand loadWorkspace = new MySqlCommand("select access_token,ConversationBlob from chats", _connection);
            var reader = await loadWorkspace.ExecuteReaderAsync();

            StringBuilder builder = new StringBuilder();
            try
            {

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        string token = reader.GetString("access_token");
                        byte[] blob = (byte[])reader["ConversationBlob"];
                        builder.Append($"{token}~~{Encoding.UTF8.GetString(blob)}^^^^^");
                    }

                    return builder.ToString();

                }
                else
                {
                    return "";
                }
            }
            finally
            {

                await reader.DisposeAsync();
                await loadWorkspace.DisposeAsync();
            }
        }




    }
}