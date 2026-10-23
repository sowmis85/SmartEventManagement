using Microsoft.Azure.Functions.Worker;
using SmartEvent.DurableFunction.Models;
using SmartEvent.DurableFunction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SmartEvent.DurableFunction.Activities
{
    public class CreateEventActivity
    {
        private readonly IEventRepository repository;

        public CreateEventActivity(IEventRepository repository)
        {
            this.repository = repository;
        }

        [Function(nameof(CreateEventActivity))]
        public async Task<string> Run(
            [ActivityTrigger] EventCreateRequest request)
        {
            var eventId = Guid.NewGuid().ToString();

            var entity = new EventCreateRequest
            {
                EventId = eventId,
                EventName = request.EventName,
                Description = request.Description,
                EventDate = request.EventDate,
                Capacity = request.Capacity,
                CreatedBy = request.CreatedBy
            };

            await repository.CreateAsync(entity);

            return eventId;
        }
    }
}
