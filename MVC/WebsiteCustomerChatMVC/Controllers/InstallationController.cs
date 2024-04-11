﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteCustomerChatMVC.Models;


namespace WebsiteCustomerChatMVC.Controllers
{
    public class InstallController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public InstallController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Install()
        {
            return View();
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