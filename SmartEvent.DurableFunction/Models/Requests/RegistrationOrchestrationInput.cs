using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Models.Requests
{
    public class RegistrationOrchestrationInput
    {
        public string EventId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public string RegistrationId { get; set; } = string.Empty;

        public int Quantity { get; set; } = 1;
    }
}
