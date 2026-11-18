using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Models.Requests
{
    public class SeatRequest
    {
        public string RegistrationId { get; set; }
        public int Quantity { get; set; }
    }
}
