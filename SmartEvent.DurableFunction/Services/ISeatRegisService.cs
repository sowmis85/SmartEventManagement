using SmartEvent.DurableFunction.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartEvent.DurableFunction.Services
{
    public interface ISeatRegisService
    {
        Task ReserveAsync(
            string eventId,
            string idempotencyKey,
            SeatRequest request,
            CancellationToken cancellationToken);
    }
}
