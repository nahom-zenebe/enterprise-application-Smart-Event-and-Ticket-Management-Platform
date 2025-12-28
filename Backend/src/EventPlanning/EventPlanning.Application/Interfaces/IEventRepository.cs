// Interfaces/IEventRepository.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IEventRepository
{
    Task AddAsync(Event @event);
    Task<Event?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Event>> ListAsync(DateTime? dateUtc, string? category, string? venue);
    Task UpdateAsync(Event @event);
    Task DeleteAsync(Event @event);
}