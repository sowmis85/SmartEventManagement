using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using SmartEvent.DurableFunction.Activities;
using SmartEvent.DurableFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Orchestrators
{
    public class EventCreationOrchestrator
    {
        [Function(nameof(EventCreationOrchestrator))]
        public async Task<EventResponse> Run(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            var request =
                context.GetInput<EventCreateRequest>();

            bool isValid =
                await context.CallActivityAsync<bool>(
                    nameof(ValidateEventActivity),
                    request);

            if (!isValid)
            {
                return new EventResponse
                {
                    Status = "Validation Failed"
                };
            }

            string eventId =
                await context.CallActivityAsync<string>(
                    nameof(CreateEventActivity),
                    request);

            await context.CallActivityAsync(
                nameof(SendNotificationActivity),
                eventId);

            return new EventResponse
            {
                EventId = eventId,
                Status = "Created"
            };
        }
    }
}
