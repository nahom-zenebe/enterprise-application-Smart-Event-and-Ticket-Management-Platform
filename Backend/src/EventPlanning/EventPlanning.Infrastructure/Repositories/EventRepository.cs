// Repositories/EventRepository.cs
using Microsoft.EntityFrameworkCore;
using System;
using EventPlanning.Infrastructure.Persistence;   
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class EventRepository : IEventRepository
{
    private readonly EventPlanningDbContext _context;

    public EventRepository(EventPlanningDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Event @event)
    {
        await _context.Events.AddAsync(@event);
        await _context.SaveChangesAsync();
    }

    public Task<Event?> GetByIdAsync(Guid id) =>
        _context.Events.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IReadOnlyList<Event>> ListAsync(DateTime? dateUtc, string? category, string? venue)
    {
        IQueryable<Event> query = _context.Events;

        if (dateUtc.HasValue)
        {
            var day = dateUtc.Value.Date;
            query = query.Where(e => e.StartDateUtc.Date == day);
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(e => e.Category == category);
        }

        if (!string.IsNullOrWhiteSpace(venue))
        {
            query = query.Where(e => e.Venue == venue);
        }

        return await query
            .OrderBy(e => e.StartDateUtc)
            .ToListAsync();
    }

    public async Task UpdateAsync(Event @event)
    {
        _context.Events.Update(@event);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Event @event)
    {
        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();
    }
}