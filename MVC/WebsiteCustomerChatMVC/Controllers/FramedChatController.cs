
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using WebsiteCustomerChatMVC.Models;

namespace WebsiteCustomerChatMVC.Controllers
{
    public class FramedChatController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IServerAddressesFeature _serverAddressesFeature;
        public FramedChatController(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _serverAddressesFeature = httpContextAccessor.HttpContext.Features.Get<IServerAddressesFeature>();
        }

        [HttpGet]
        public IActionResult FramedChat()
        {
            var addresses = _serverAddressesFeature?.Addresses.ToList() ?? new List<string> { "Address not found" };

            addresses.ForEach(x =>
            {
                Console.WriteLine(x);
            });
            // Get the wwwroot path
            //string webRootPath = AppDomain.CurrentDomain.BaseDirectory+"/wwwroot";
            // Console.WriteLine(webRootPath);
            // Specify the relative path to your HTML file within wwwroot
            // string relativeFilePath = "FramedChat/FramedChat.html"; // Example path

            // Combine wwwroot path with the relative file path
            // string filePath = Path.Combine(webRootPath, relativeFilePath);

            // Check if the file exists
            // if (!System.IO.File.Exists(filePath))
            // {
            //     return NotFound();
            // }

            // Return the HTML file using PhysicalFileResult
            // return PhysicalFile(filePath, "text/html");
            return View(new FramedChatModel(addresses[0]));
        }

    }
}
