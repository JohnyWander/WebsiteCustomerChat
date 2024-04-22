using System.Diagnostics;
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using WebsiteCustomerChatMVC.ViewTostring;
using WebsiteCustomerChatConfiguration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using WebsiteCustomerChatMVC.SignarR.Hubs;
using WebsiteCustomerChatMVC.SignarR;

namespace WebsiteCustomerChatMVC
{
    public class Program
    {
        public static void Init()
        {
            bool ConfigExists = File.Exists(Config.configFilename);
            if (ConfigExists)
            {
                Config.ParseConfig();
                string dbEngine = Config.GetConfigValue("DatabaseEngine");
                
                string DbName = Config.GetConfigValue("DatabaseName");
                string DbServer = Config.GetConfigValue("DatabaseHost");
                string dbport = Config.GetConfigValue("DatabasePort");
                string dbuser = Config.GetConfigValue("DatabaseUser");
                string dbpassword = Config.GetConfigValue("DatabasePassword");
                IDBcheckOnInit dbcontecx = dbEngine switch
                {
                    "sqlite" => new SQLITEengine(DbName),
                    "mysql" => new MySQLengine(DbServer,dbport,DbName,dbuser,dbpassword)
                };

                if (!dbcontecx.DatabaseExists())
                {
                    dbcontecx.CheckForDbOrCreate().Wait();
                }


            //    dbcontecx.Dispose();




            }
            else
            {
                return; // Assume that wcc is not installed and proceed with normal startup
            }
            Debug.WriteLine(ConfigExists);
        }


        public static void Main(string[] args)
        {
            Init();


            //SQLITEengine engine = new SQLITEengine(AppDomain.CurrentDomain.BaseDirectory+"TEST.DB");
            // engine.Database.EnsureCreated();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR();
            builder.Services.AddTransient<ChatModule>();
            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));


            //builder.Services.AddScoped<IViewRenderer, ViewRenderer>();
            var app = builder.Build();




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Install",
                pattern: "{controller=Install}/{action=Install}/{id?}"
                );
            app.MapControllerRoute(
                name: "LoggedIn",
                pattern: "{controller=LoggedIn}/{action=LoggedIn}/{id?}"
                ); ;

            app.UseCors("CorsPolicy");
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/Chathub");
            });

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapHub<AdminChatHub>("/ChatterHub");
            });

           
            app.Run();
        }
    }
}