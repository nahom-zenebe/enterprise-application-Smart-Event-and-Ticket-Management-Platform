using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface IOutboxService
    {
        Task SaveEventAsync(string eventType, object eventData);
        Task<IEnumerable<OutboxEvent>> GetUnprocessedEventsAsync();
        Task MarkAsProcessedAsync(Guid eventId);
    }
}