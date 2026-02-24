using Application;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace MyTestFunctionApp
{
    public class WeatherApp
    {
        private readonly ILogger<WeatherApp> _logger;
        private readonly IMyClient _myClient;

        public WeatherApp(ILogger<WeatherApp> logger, IMyClient myClient)
        {
            _logger = logger;
            _myClient = myClient;
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
                    "/api/getmydata?ques=What is DevOps?",
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
                var ques = query.TryGetValue("ques", out var lowerQuestion)
                    ? lowerQuestion.ToString()
                    : query.TryGetValue("Ques", out var upperQuestion)
                        ? upperQuestion.ToString()
                        : string.Empty;

                if (string.IsNullOrWhiteSpace(ques))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Headers.Add("Content-Type", "application/json");
                    await response.WriteStringAsync(JsonSerializer.Serialize(new
                    {
                        message = "Please pass a non-empty query parameter 'ques'."
                    }));

                    return response;
                }

                var answer = await _myClient.AskQuestionAsync(ques, req.FunctionContext.CancellationToken);

                var resultObject = new
                {
                    question = ques,
                    answer
                };

                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonSerializer.Serialize(resultObject));

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetMyData");

                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Headers.Add("Content-Type", "application/json");
                await response.WriteStringAsync(JsonSerializer.Serialize(new
                {
                    message = "Error occurred while querying the model."
                }));

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
