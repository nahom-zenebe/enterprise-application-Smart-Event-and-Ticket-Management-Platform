using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerExperience.Domain.Entities;

namespace CustomerExperience.Application.Interfaces
{
    public interface IEventInteractionRepository
    {
        // Basic CRUD
        Task<EventInteraction?> GetByIdAsync(Guid id);
        Task<EventInteraction> CreateAsync(EventInteraction interaction);
        Task<bool> DeleteAsync(Guid id);
        
        // User Interactions
        Task<EventInteraction?> GetUserEventInteractionAsync(Guid userId, Guid eventId, InteractionType interactionType);
        Task<IEnumerable<EventInteraction>> GetUserInteractionsAsync(Guid userId, InteractionType? interactionType = null);
        Task<IEnumerable<EventInteraction>> GetEventInteractionsAsync(Guid eventId, InteractionType? interactionType = null);
        
        // Check if interaction exists
        Task<bool> HasInteractionAsync(Guid userId, Guid eventId, InteractionType interactionType);
        
        // Remove specific interaction
        Task<bool> RemoveInteractionAsync(Guid userId, Guid eventId, InteractionType interactionType);
        
        // Stats
        Task<int> GetInteractionCountAsync(Guid eventId, InteractionType interactionType);
        Task<Dictionary<InteractionType, int>> GetEventStatsAsync(Guid eventId);
        
        // User favorites
        Task<IEnumerable<EventInteraction>> GetUserSavedEventsAsync(Guid userId);
        
        // Recent interactions
        Task<IEnumerable<EventInteraction>> GetRecentInteractionsAsync(Guid userId, int limit = 10);
    }
}