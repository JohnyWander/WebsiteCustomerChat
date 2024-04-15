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
using WebsiteCustomerChatConfiguration;


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebsiteCustomerChatMVC.Controllers
{
    public class InstallController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IViewRenderer _viewRenderer;
        /*
        public InstallController(ILogger<HomeController> logger, IViewRenderer viewRenderer)
        {
            _logger = logger;
            _viewRenderer = viewRenderer;
        }
        */
        public InstallController(ILogger<HomeController> logger)
        {
            _logger = logger;
           
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


                //string viewString = RenderRazorViewToString(this, "/Home/Privacy", new InstallationViewModel(form)); /doesnt work
                var model = new InstallationViewModel(form);
                string viewString = await RenderViewToStringAsync("Install", new InstallationViewModel(form), ControllerContext);
                await Response.WriteAsync(viewString);
                //await Task.Delay(5000);
                await Response.WriteAsync("Creating Config File...\n");
                await model.Setconfiguration();
                await Task.Delay(5000);
                await Config.BuildConfigFile();
                await Response.WriteAsync("Values Set, conf file created,\nTrying to parse Configuration...\n");
                await Response.WriteAsync(await model.TryParseConfig()+"\n");
                await Response.WriteAsync("Trying to create database ... \n");
                await Response.WriteAsync(await model.EnsureOrCreate()+"\n");






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



        public  string RenderRazorViewToString(Controller controller, string viewName, object model)
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

        public static async Task<string> RenderViewToStringAsync(
    string viewName, object model,
    ControllerContext controllerContext,
    bool isPartial = false)
        {
            var actionContext = controllerContext as ActionContext;

            var serviceProvider = controllerContext.HttpContext.RequestServices;
            var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            using (var sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, viewName, !isPartial);

                if (viewResult?.View == null)
                    throw new ArgumentException($"{viewName} does not match any available view");

                var viewDictionary =
                    new ViewDataDictionary(new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    { Model = model };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }



    } }