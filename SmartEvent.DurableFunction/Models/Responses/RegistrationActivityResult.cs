using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Models.Responses
{
    public class RegistrationActivityResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; } = string.Empty;

        public string ErrorDetails { get; set; }
        public string RegistrationId { get; set; }
        public string EventId { get; set; }
    }
}
