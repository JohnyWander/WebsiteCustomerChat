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

namespace WebsiteCustomerChatMVC.Models
{
    public class InstallationViewModel
    {
        
        private string ?AdminUsername;
        private string ?AdminPassword;
        private string ?AdminConfirmPassword;

        

        private string? dbengine;
        private string? sqlitedbFile;

        private string? server;
        private string? port;
        private string? dbname;
        private string? dbuser;
        private string? dbuserpassword;

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



        }

        public string EnsureOrCreate()
        {
            try
            {
                
                DBengine.CheckForDbOrCreate();
               
                return "Success";
            }catch(Exception ex)
            {
                return $"DB creation failed with exception - {ex.Message}";
            }
        }


    }
}
