using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.Security.Cryptography;
using WebsiteCustomerChatMVC.Models;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using WebsiteCustomerChatConfiguration;
using System.Reflection;

namespace WebsiteCustomerChatMVC.Models
{
    public class InstallationViewModel
    {
        
        private string AdminUsername;
        private string AdminPassword;
        private string AdminConfirmPassword;



        private string dbengine;
        private string sqlitedbFile;

        private string server;
        private string port;
        private string dbname;
        private string dbuser;
        private string dbuserpassword;

        internal IDbInstallation DBengine;

       

        public InstallationViewModel(IFormCollection form)
        {
           
            AdminUsername = form["username"];
            AdminPassword = form["password"];
            AdminConfirmPassword = form["confirmPassword"];
            dbengine = form["dbengine"];
            
            if(dbengine == "sqlite")
            {
                sqlitedbFile = form["sqlitedbFile"];
                DBengine = new SQLITEengine(sqlitedbFile);
                
            }
            else if(dbengine == "mysql")
            {
                server = form["server"];
                port = form["port"];
                dbname = form["name"];
                dbuser = form["user"];
                dbuserpassword = form["password"];

                DBengine = new MySQLengine(server,port,dbname,dbuser,dbuserpassword);
                
            }


            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            PropertyInfo[] properties = type.GetProperties();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    string value = (string)field.GetValue(this);
                    Console.WriteLine($"{field.Name}: {value}");
                }
            }

        }

        public async Task Setconfiguration()
        {
      
           await Config.SetConfigValue("DatabaseEngine", dbengine);
            if (this.dbengine == "mysql")
            {
                await Config.SetConfigValue("DatabaseName", this.dbname);
            }
            else {
                await Config.SetConfigValue("DatabaseName", this.sqlitedbFile);
            }
            await Config.SetConfigValue("DatabaseUser", this.dbuser);
            await Config.SetConfigValue("DatabasePassword", this.dbuserpassword);
            await Config.SetConfigValue("DatabaseHost", this.server);
            await Config.SetConfigValue("DatabasePort", this.port);

                   
        }

        public async Task<string> TryParseConfig()
        {
            try
            {
                await Config.ParseConfig();
                return "Success!";
            }
            catch (ConfigurationException ex)
            {
                return $"Parsing configuration failed with error - {ex.GetType().Name} - {ex.Message}";
            }

        }

        public async Task<string> EnsureOrCreate()
        {
            try
            {
                
                await DBengine.CheckForDbOrCreate();
               
                return "Success";
            }catch(Exception ex)
            {
                return $"DB creation failed with exception - {ex.Message}";
            }
        }


    }
}
