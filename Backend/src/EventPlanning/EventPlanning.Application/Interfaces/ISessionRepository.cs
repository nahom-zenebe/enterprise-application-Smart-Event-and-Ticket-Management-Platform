using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlanning.Domain.Entities;

namespace EventPlanning.Application.Interfaces
{
    public interface ISessionRepository
    {
        Task<Session> CreateAsync(Session session);
        Task<List<Session>> GetByEventIdAsync(Guid eventId);
        Task<Session?> GetByIdAsync(Guid id);
        Task<Session> UpdateAsync(Session session);
        Task<bool> DeleteAsync(Guid id);
    }
}