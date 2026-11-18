using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using SmartEvent.DurableFunction.Models.Requests;
using SmartEventDurableFunction.Orchestrators;
using System.Net;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.HttpTriggers
{
    public class EventCreationStarter
    {
        [Function(nameof(EventCreationStarter))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post",
        Route = "events")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            var request =
                await req.ReadFromJsonAsync<EventCreateRequest>();

            string instanceId =
                await client.ScheduleNewOrchestrationInstanceAsync(
                    nameof(RegistrationOrchestrator),
                    request);

            var response = req.CreateResponse(HttpStatusCode.Accepted);

            await response.WriteAsJsonAsync(new
            {
                InstanceId = instanceId
            });

            return response;
        }
    }   
}
