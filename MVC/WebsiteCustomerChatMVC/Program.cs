using System.Diagnostics;
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using WebsiteCustomerChatMVC.ViewTostring;
using WebsiteCustomerChatConfiguration;

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
                string dbEngine = Config.ParsedConfiguration.FirstOrDefault(x => x.Name == "DatabaseEngine").Value;
                string DbName = Config.ParsedConfiguration.FirstOrDefault(x => x.Name == "DatabaseName").Value;
                IDBcheckOnInit dbcontecx = dbEngine switch
                {
                    "sqlite" => new SQLITEengine(DbName),
                    "mysql" => new MySQLengine(DbName)
                };
              
                    
                    
                    

                
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

           
            app.Run();
        }
    }
}