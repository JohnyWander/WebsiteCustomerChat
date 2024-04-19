using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WccEntityFrameworkDriver.DatabaseEngineOperations;
using WccEntityFrameworkDriver.DatabaseEngineOperations.Interfaces;
using WebsiteCustomerChatMVC.Models;
using WebsiteCustomerChatConfiguration;
using WccEntityFrameworkDriver.DatabaseEngineOperations.DataSets;

namespace WebsiteCustomerChatMVC.Controllers
{
    public class LoggedInController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IDBLogin db;

        public LoggedInController(ILogger<HomeController> logger)
        {
            _logger = logger;

            string engine = Config.GetConfigValue("DatabaseEngine");
            if(engine == "mysql")
            {
                db = new MySQLengine();
            }
            else
            {
                db = new SQLITEengine(Config.GetConfigValue("DatabaseName"));
            }

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

            LoggedUserContext user =await db.UserLogin(Username, Password);
            if(user.NoUserContext == true)
            {
                return Redirect("/?Badlogin=yes");
            }



            return View();
        }

    }
}
