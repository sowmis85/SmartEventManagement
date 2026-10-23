using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartEvent.DurableFunction.Models;

namespace SmartEvent.DurableFunction.Activities
{
    public class ValidateEventActivity
    {
        [Function(nameof(ValidateEventActivity))]
        public bool Run(
            [ActivityTrigger] EventCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EventName))
                return false;

            if (request.EventDate <= DateTime.UtcNow)
                return false;

            if (request.Capacity <= 0)
                return false;

            return true;
        }
    }
}
