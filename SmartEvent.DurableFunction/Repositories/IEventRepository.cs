using SmartEvent.DurableFunction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace SmartEvent.DurableFunction.Repositories
{
    public interface IEventRepository
    {
        Task CreateAsync(EventCreateRequest entity);

        Task<EventCreateRequest> GetAsync(string eventId);
    }

    
}
