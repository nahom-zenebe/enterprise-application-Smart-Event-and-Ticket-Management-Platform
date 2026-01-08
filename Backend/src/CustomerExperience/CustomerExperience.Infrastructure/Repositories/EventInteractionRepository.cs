using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerExperience.Application.Interfaces;
using CustomerExperience.Domain.Entities;
using CustomerExperience.Infrastructure.Persistence;

namespace CustomerExperience.Infrastructure.Repositories
{
    public class EventInteractionRepository : IEventInteractionRepository
    {
        private readonly CustomerExperienceDbContext _context;

        public EventInteractionRepository(CustomerExperienceDbContext context)
        {
            _context = context;
        }

        public async Task<EventInteraction?> GetByIdAsync(Guid id)
        {
            return await _context.EventInteractions.FindAsync(id);
        }

        public async Task<EventInteraction> CreateAsync(EventInteraction interaction)
        {
            // Check if similar interaction already exists
            var existing = await _context.EventInteractions
                .FirstOrDefaultAsync(e => 
                    e.UserId == interaction.UserId && 
                    e.EventId == interaction.EventId && 
                    e.InteractionType == interaction.InteractionType);
            
            if (existing != null)
            {
                // Update timestamp if exists
                existing.Timestamp = DateTime.UtcNow;
                existing.Metadata = interaction.Metadata;
                await _context.SaveChangesAsync();
                return existing;
            }

            _context.EventInteractions.Add(interaction);
            await _context.SaveChangesAsync();
            return interaction;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var interaction = await _context.EventInteractions.FindAsync(id);
            if (interaction == null) return false;
            
            _context.EventInteractions.Remove(interaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EventInteraction?> GetUserEventInteractionAsync(
            Guid userId, Guid eventId, InteractionType interactionType)
        {
            return await _context.EventInteractions
                .FirstOrDefaultAsync(e => 
                    e.UserId == userId && 
                    e.EventId == eventId && 
                    e.InteractionType == interactionType);
        }

        public async Task<IEnumerable<EventInteraction>> GetUserInteractionsAsync(
            Guid userId, InteractionType? interactionType = null)
        {
            var query = _context.EventInteractions
                .Where(e => e.UserId == userId);
            
            if (interactionType.HasValue)
            {
                query = query.Where(e => e.InteractionType == interactionType.Value);
            }
            
            return await query
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventInteraction>> GetEventInteractionsAsync(
            Guid eventId, InteractionType? interactionType = null)
        {
            var query = _context.EventInteractions
                .Where(e => e.EventId == eventId);
            
            if (interactionType.HasValue)
            {
                query = query.Where(e => e.InteractionType == interactionType.Value);
            }
            
            return await query.ToListAsync();
        }

        public async Task<bool> HasInteractionAsync(
            Guid userId, Guid eventId, InteractionType interactionType)
        {
            return await _context.EventInteractions
                .AnyAsync(e => 
                    e.UserId == userId && 
                    e.EventId == eventId && 
                    e.InteractionType == interactionType);
        }

        public async Task<bool> RemoveInteractionAsync(
            Guid userId, Guid eventId, InteractionType interactionType)
        {
            var interaction = await _context.EventInteractions
                .FirstOrDefaultAsync(e => 
                    e.UserId == userId && 
                    e.EventId == eventId && 
                    e.InteractionType == interactionType);
            
            if (interaction == null) return false;
            
            _context.EventInteractions.Remove(interaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetInteractionCountAsync(
            Guid eventId, InteractionType interactionType)
        {
            return await _context.EventInteractions
                .CountAsync(e => 
                    e.EventId == eventId && 
                    e.InteractionType == interactionType);
        }

        public async Task<Dictionary<InteractionType, int>> GetEventStatsAsync(Guid eventId)
        {
            var stats = await _context.EventInteractions
                .Where(e => e.EventId == eventId)
                .GroupBy(e => e.InteractionType)
                .Select(g => new { Type = g.Key, Count = g.Count() })
                .ToListAsync();
            
            var result = Enum.GetValues(typeof(InteractionType))
                .Cast<InteractionType>()
                .ToDictionary(type => type, type => 0);
            
            foreach (var stat in stats)
            {
                result[stat.Type] = stat.Count;
            }
            
            return result;
        }

        public async Task<IEnumerable<EventInteraction>> GetUserSavedEventsAsync(Guid userId)
        {
            return await _context.EventInteractions
                .Where(e => e.UserId == userId && e.InteractionType == InteractionType.Saved)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventInteraction>> GetRecentInteractionsAsync(
            Guid userId, int limit = 10)
        {
            return await _context.EventInteractions
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Timestamp)
                .Take(limit)
                .ToListAsync();
        }
    }
}