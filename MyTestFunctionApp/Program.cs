using Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.ApplicationInsights.WorkerService;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // Application Insights
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Ollama Chat Client
        services.AddChatClient(new OllamaChatClient(
            new Uri("http://localhost:11434"),
            "llama3"
        ));

        // Your custom services
        services.AddScoped<IMyClient, MyClient>();

        // HttpClient factory
        services.AddHttpClient();
    })
    .Build();

host.Run();