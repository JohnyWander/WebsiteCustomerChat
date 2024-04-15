using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Org.BouncyCastle.Asn1.X509;
using System.Diagnostics;
using System.Text;
using WebsiteCustomerChatMVC.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;

namespace WebsiteCustomerChatMVC.Controllers
{
    public class InstallController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IViewRenderer _viewRenderer;

        public InstallController(ILogger<HomeController> logger, IViewRenderer viewRenderer)
        {
            _logger = logger;
            _viewRenderer = viewRenderer;
        }

        public async Task<IActionResult> Install()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Install(IFormCollection form)
        {


            Request.HttpContext.Response.OnStarting(async () =>
            {


                string viewString = RenderRazorViewToString(this, "/Home/Privacy", new InstallationViewModel(form));
                await Response.WriteAsync(viewString);





            });
            return Ok(); ;

        }

        /*
        [HttpPost]      
        public async Task<IActionResult> Install(IFormCollection form)
        {
            Request.HttpContext.Response.OnStarting(async () =>
            {
              await Response.WriteAsync("XD");
                await Response.Body.FlushAsync();
                await Task.Delay(1000);
               // await Response.
            });
           
            

            return View(new InstallationViewModel(form));           
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

        }



        public static string RenderRazorViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                IViewEngine? viewEngine =
                    controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                        ICompositeViewEngine;
                ViewEngineResult? viewResult = viewEngine?.FindView(controller.ControllerContext, viewName, false);

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    sw,
                    new HtmlHelperOptions()
                );
                viewResult.View.RenderAsync(viewContext);
                return sw.GetStringBuilder().ToString();
            }
        }
    } }