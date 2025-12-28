using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlanning.Application.Interfaces;
using EventPlanning.Domain.Entities;
using EventPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Infrastructure.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly EventPlanningDbContext _context;

        public SessionRepository(EventPlanningDbContext context)
        {
            _context = context;
        }

        public async Task<Session> CreateAsync(Session session)
        {
            // Ensure CreatedAt is set
            if (session.CreatedAt == default)
                session.CreatedAt = DateTime.UtcNow;
                
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<List<Session>> GetByEventIdAsync(Guid eventId)
        {
            return await _context.Sessions
                .Where(s => s.EventId == eventId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Session?> GetByIdAsync(Guid id)
        {
            return await _context.Sessions.FindAsync(id);
        }

        public async Task<Session> UpdateAsync(Session session)
        {
            session.UpdatedAt = DateTime.UtcNow;
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
                return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}