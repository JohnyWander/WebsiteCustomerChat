using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteCustomerChatMVC.Models;

namespace WebsiteCustomerChatMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View(new IndexViewModel((Request.Query.Count > 0) && (Request.Query["Badlogin"] == "yes") ? true : false));
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}