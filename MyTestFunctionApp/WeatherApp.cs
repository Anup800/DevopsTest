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

        [Function("ApiRoot")]
        public async Task<HttpResponseData> ApiRoot(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "")] HttpRequestData req)
        {
            _logger.LogInformation("Api root triggered");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");

            var payload = new
            {
                message = "Function app is running",
                endpoints = new[]
                {
                    "/api/getmydata?Ques=hello",
                    "/api/getmydata2"
                }
            };

            await response.WriteStringAsync(JsonSerializer.Serialize(payload));
            return response;
        }

        [Function("GetMyData")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getmydata")] HttpRequestData req)
        {
            _logger.LogInformation("GetMyData triggered");

            var response = req.CreateResponse(HttpStatusCode.OK);

            try
            {
                var query = QueryHelpers.ParseQuery(req.Url.Query);
                var ques = query.TryGetValue("Ques", out var question) ? question.ToString() : string.Empty;

                var resultObject = new
                {
                    message = "Hello",
                    question = ques
                };

                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonSerializer.Serialize(resultObject));

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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getmydata2")] HttpRequestData req)
        {
            _logger.LogInformation("GetMyData2 triggered");

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync("Run2 executed successfully");

            return response;
        }
    }
}
