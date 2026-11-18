using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using SmartEvent.DurableFunction.Common;
using SmartEvent.DurableFunction.Services;
using System;
using System.Runtime.Remoting.Contexts;

namespace SmartEvent.DurableFunction
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((context, services) =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();

                    // Register HttpClient
                    var apiUrl = context.Configuration["ApiSettings:HackDemoApiUrl"];
                    if (apiUrl == null)
                    {
                        throw new InvalidOperationException(ValidationConstant.URLConfigurationError);
                    }
                    services.AddHttpClient<ISeatRegisService, SeatRegisService>(
                     client =>
                     {
                         client.BaseAddress = new Uri(apiUrl);
                     });
                })
                .Build();

            host.Run();
        }
    }
}