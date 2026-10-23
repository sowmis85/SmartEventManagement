using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Models
{
    public class EventResponse
    {
        public string EventId { get; set; }
        public string Status { get; set; }
    }
}
