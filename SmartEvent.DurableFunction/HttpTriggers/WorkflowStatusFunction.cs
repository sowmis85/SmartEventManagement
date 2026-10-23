using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.HttpTriggers
{
    public class WorkflowStatusFunction
    {
        [Function(nameof(WorkflowStatusFunction))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function,
        "get",
        Route = "events/workflows/{instanceId}")]
        HttpRequestData req,
            string instanceId,
            [DurableClient] DurableTaskClient client)
        {
            var status =
                await client.GetInstanceAsync(instanceId);

            var response = req.CreateResponse(
                HttpStatusCode.OK);

            await response.WriteAsJsonAsync(status);

            return response;
        }
    }
}
