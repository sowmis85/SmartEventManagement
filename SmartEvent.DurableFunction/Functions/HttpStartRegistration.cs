using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using SmartEvent.DurableFunction.Common;
using SmartEvent.DurableFunction.Models.Requests;
using SmartEventDurableFunction.Orchestrators;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartEventDurableFunction.Functions
{
    public static class HttpStartRegistration
    {
        [Function(nameof(HttpStartRegistration))]
        public static async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orchestrators/RegistrationOrchestrator")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger(nameof(HttpStartRegistration));

            logger.LogInformation(ValidationConstant.HTTPStarterMessage);

            try
            {
                // Parse request body
                var requestBody = await req.ReadAsStringAsync();
                var input = JsonSerializer.Deserialize<RegistrationOrchestrationInput>(
                    requestBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (input == null)
                {
                    logger.LogError(ValidationConstant.RequestBodyParseError);
                    var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await errorResponse.WriteAsJsonAsync(new { error = ValidationConstant.RequestBodyParseError });
                    return errorResponse;
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(input.EventId) ||
                    string.IsNullOrWhiteSpace(input.UserId) ||
                    string.IsNullOrWhiteSpace(input.RegistrationId))
                {
                    logger.LogError(ValidationConstant.MissingIDInputParameters);
                    var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                    await errorResponse.WriteAsJsonAsync(
                        new { error = ValidationConstant.MissingIDInputParameters });
                    return errorResponse;
                }

                // Start the orchestration
                var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                    nameof(RegistrationOrchestrator),
                    input);

                logger.LogInformation(ValidationConstant.OrchestrationInstanceId, instanceId);

                // Return status URLs
                return client.CreateCheckStatusResponse(req, instanceId);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex, ValidationConstant.OrchestratorStartFailed);

                var errorResponse = req.CreateResponse(HttpStatusCode.InternalServerError);
                await errorResponse.WriteAsJsonAsync(
                    new { error = ValidationConstant.OrchestratorStartFailed });
                return errorResponse;
            }
        }
    }
}