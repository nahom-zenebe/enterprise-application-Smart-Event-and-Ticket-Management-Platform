using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Persistence;

namespace Ticketing.Infrastructure.Services
{
    public class OutboxService : IOutboxService
    {
        private readonly TicketingDbContext _context;

        public OutboxService(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task SaveEventAsync(string eventType, object eventData)
        {
            var outboxEvent = new OutboxEvent
            {
                Id = Guid.NewGuid(),
                EventType = eventType,
                EventData = JsonSerializer.Serialize(eventData),
                CreatedAt = DateTime.UtcNow,
                IsProcessed = false
            };

            await _context.OutboxEvents.AddAsync(outboxEvent);
        }

        public async Task<IEnumerable<OutboxEvent>> GetUnprocessedEventsAsync()
        {
            return await _context.OutboxEvents
                .Where(e => !e.IsProcessed)
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<OutboxEvent>> GetRetryableEventsAsync()
        {
            return await _context.OutboxEvents
                .Where(e => !e.IsProcessed && 
                           e.RetryCount < e.MaxRetries && 
                           (e.NextRetryAt == null || e.NextRetryAt <= DateTime.UtcNow))
                .OrderBy(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task MarkAsProcessedAsync(Guid eventId)
        {
            var outboxEvent = await _context.OutboxEvents.FindAsync(eventId);
            if (outboxEvent != null)
            {
                outboxEvent.IsProcessed = true;
                outboxEvent.ProcessedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsFailedAsync(Guid eventId, string error)
        {
            var outboxEvent = await _context.OutboxEvents.FindAsync(eventId);
            if (outboxEvent != null)
            {
                outboxEvent.RetryCount++;
                outboxEvent.LastError = error;
                outboxEvent.NextRetryAt = DateTime.UtcNow.AddMinutes(Math.Pow(2, outboxEvent.RetryCount));
                await _context.SaveChangesAsync();
            }
        }
    }
}