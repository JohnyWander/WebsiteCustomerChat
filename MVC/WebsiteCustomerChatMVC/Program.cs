
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WebsiteCustomerChatMVC.ViewTostring;

namespace WebsiteCustomerChatMVC
{
    public class Program
    {
        
        public static void Main(string[] args)
        {

           

            //SQLITEengine engine = new SQLITEengine(AppDomain.CurrentDomain.BaseDirectory+"TEST.DB");
           // engine.Database.EnsureCreated();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddRazorPages();
            //builder.Services.AddScoped<IViewRenderer, ViewRenderer>();
            var app = builder.Build();

            
            //app.UseWhen(context => context.Request.Path.StartsWithSegments("/install"), builder =>
            //{
/*
                app.Use(async (context, next) =>
                {
                    context.Response.OnStarting(async() =>
                    {
                        
                        await context.Response.WriteAsync(await source.Task);
                        source = new TaskCompletionSource<string>();
                       // return Task.FromResult(0);
                    });

                    await next();
                });
*/
           // });
            

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