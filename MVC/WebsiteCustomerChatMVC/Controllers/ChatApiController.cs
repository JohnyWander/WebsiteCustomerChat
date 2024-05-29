using Microsoft.AspNetCore.Mvc;

namespace WebsiteCustomerChatMVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatApiController : Controller
    {

        private static readonly string[] Summaries = new[]
     {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<ChatApiController> _logger;

        public ChatApiController(ILogger<ChatApiController> logger)
        {
            _logger = logger;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }







    }
}
