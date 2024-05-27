using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteCustomerChatMVC.Models;
using WebsiteCustomerChatConfiguration;
using WebsiteCustomerChatMVC.SignarR;
using WebsiteCustomerChatMVC.DatabaseNoEF.MySql;
using WebsiteCustomerChatMVC.DataSets;


namespace WebsiteCustomerChatMVC.Controllers
{
    public class LoggedInController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ChatModule _chatModule;


        MysqlUserEngine dbnoef = new MysqlUserEngine();
        public LoggedInController(ILogger<HomeController> logger,ChatModule chatmodule)
        {
            _logger = logger;
            _chatModule = chatmodule;

            

        }


        [HttpGet]
        public IActionResult LoggedIn()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoggedIn(IFormCollection form)
        {
            
            string Username = form["username"];
            string Password = form["password"];

            LoggedUserContext user =await dbnoef.UserLogin(Username, Password);
            
            if (user.NoUserContext == true)
            {
                await dbnoef.DisposeAsync();
                return Redirect("/?Badlogin=yes");
            }
            else
            {
                await dbnoef.DisposeAsync();
                _chatModule.StartDBConnection();
            }


            


            return View();
        }

    }
}
