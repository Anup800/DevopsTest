using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

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
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("GetMyData triggered");

            var response = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                var query = QueryHelpers.ParseQuery(req.Url.Query);
                var ques = query["Ques"].ToString();

                var resultObject = new
                {
                    message = "Hello",
                    question = ques
                };

                response.Headers.Add("Content-Type", "application/json");

                await response.WriteStringAsync(JsonSerializer.Serialize("helo"));

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyData");

                response.StatusCode = HttpStatusCode.InternalServerError;
                await response.WriteStringAsync("Error occurred");

                return response;
            }
        }

        [Function("GetMyData2")]
        public async Task<HttpResponseData> Run2(
            [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation("GetMyData2 triggered");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Run2 executed successfully");

            return response;
        }
    }
}