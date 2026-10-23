using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Models
{
    public class EventCreateRequest
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public int Capacity { get; set; }
        public string CreatedBy { get; set; }
    }
}
