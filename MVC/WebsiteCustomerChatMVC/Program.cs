
using WebsiteCustomerChatMVC.ViewTostring;
using WebsiteCustomerChatConfiguration;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using WebsiteCustomerChatMVC.SignarR.Hubs;
using WebsiteCustomerChatMVC.SignarR;
using System;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;

namespace WebsiteCustomerChatMVC
{
    public class Program
    {
        public static void Init(out bool ConfigExists)
        {
            ConfigExists = File.Exists(Config.configFilename);
            if (ConfigExists)
            {

                Config.ParseConfig();
                string dbEngine = Config.GetConfigValue("DatabaseEngine");
                
                string DbName = Config.GetConfigValue("DatabaseName");
                string DbServer = Config.GetConfigValue("DatabaseHost");
                string dbport = Config.GetConfigValue("DatabasePort");
                string dbuser = Config.GetConfigValue("DatabaseUser");
                string dbpassword = Config.GetConfigValue("DatabasePassword");           

            }
            else
            {
                return; // Assume that wcc is not installed and proceed with normal startup
            }
            
        }


        public static void Main(string[] args)
        {
            bool IsInstalled;
            Init(out IsInstalled);

            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRazorPages();
            builder.Services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMilliseconds(10000);

            }).AddHubOptions<AdminChatHub>(o =>
            {
                o.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                o.KeepAliveInterval = TimeSpan.FromSeconds(15);
                
            });
            
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<ChatModule>();


            builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true)
                       .AllowCredentials();
            }));

            if (!builder.Environment.IsDevelopment())
            {
                
               
                if (IsInstalled) {
                    ConfigData certConf = Config.ParsedConfiguration.FirstOrDefault(c => c.Name == "Pfx certificate Path");
                    ConfigData certPass = Config.ParsedConfiguration.FirstOrDefault(c => c.Name == "Certificate password");
                        if (certConf.Value != null && certConf.Value !="none")
                        {
                            var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions
                            {
                                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                                ClientCertificateMode = ClientCertificateMode.AllowCertificate,
                                ServerCertificate = new X509Certificate2(certConf.Value, certPass.Value)
                            };

                            builder.WebHost.ConfigureKestrel(options =>
                            options.ConfigureEndpointDefaults(listenOptions =>
                            listenOptions.UseHttps(httpsConnectionAdapterOptions)));
                        }
                }


                ConfigData endpoint = Config.ParsedConfiguration.FirstOrDefault(c => c.Name == "ListenOn");

                if(endpoint.Value != null && endpoint.Value != "")
                {
                //    builder.WebHost.UseUrls(endpoint.Value);
                } // or asp net defaults

            }

                //builder.Services.AddSingleton(hubContext);
                //builder.Services.AddSingleton(adminContext);            

            //builder.Services.AddScoped<IViewRenderer, ViewRenderer>();
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                
            }
            else
            {
                Console.WriteLine("SUCC");
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

            app.MapControllerRoute(
                name: "Framed",
                pattern: "{controller=FramedChat}/{action=FramedChat}/{id?}"
                );


            app.UseCors("CorsPolicy");
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/Chathub");
            });

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapHub<AdminChatHub>("/ChatterHub");
                
            });

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapHub<CheckIfOnlineHub>("/online");
            });
           


            app.Run();
        }
    }
}