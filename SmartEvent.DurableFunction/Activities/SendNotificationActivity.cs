using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Activities
{
    public class SendNotificationActivity
    {
        [Function(nameof(SendNotificationActivity))]
        public async Task Run(
            [ActivityTrigger] string eventId)
        {
            // Email, Teams, SMS, etc.

            Console.WriteLine(
                $"Event Created Successfully: {eventId}");

            await Task.CompletedTask;
        }
    }
}
