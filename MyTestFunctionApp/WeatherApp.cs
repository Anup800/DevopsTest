using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MyTestFunctionApp
{
    public class WeatherApp
    {
        private readonly ILogger<WeatherApp> _logger;

        public WeatherApp(ILogger<WeatherApp> logger)
        {
            _logger = logger;
        }

        [Function("GetMyData")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
