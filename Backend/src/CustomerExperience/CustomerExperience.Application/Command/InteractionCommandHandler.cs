//InteractionCommandHandler.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerExperience.Application.DTOs;
using CustomerExperience.Application.Interfaces;
using CustomerExperience.Domain.Entities;

namespace CustomerExperience.Application.Commands
{
    public interface IEventInteractionService
    {
        Task<InteractionResponseDto> CreateInteractionAsync(Guid userId, CreateInteractionDto dto);
        Task<InteractionResponseDto> GetInteractionByIdAsync(Guid id);
        Task<IEnumerable<InteractionResponseDto>> GetUserInteractionsAsync(Guid userId);
        Task<bool> DeleteInteractionAsync(Guid id);
        Task<EventStatsDto> GetEventStatsAsync(Guid eventId);
        Task<IEnumerable<EventInteractionDto>> GetUserFavoritesAsync(Guid userId);
    }

    public class EventInteractionService : IEventInteractionService
    {
        private readonly IEventInteractionRepository _repository;

        public EventInteractionService(IEventInteractionRepository repository)
        {
            _repository = repository;
        }

        public async Task<InteractionResponseDto> CreateInteractionAsync(Guid userId, CreateInteractionDto dto)
        {
            var interaction = new EventInteraction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EventId = dto.EventId,
                InteractionType = dto.InteractionType,
                Metadata = dto.Metadata,
                Timestamp = DateTime.UtcNow
            };

            var createdInteraction = await _repository.CreateAsync(interaction);

            return MapToDto(createdInteraction);
        }

        public async Task<InteractionResponseDto> GetInteractionByIdAsync(Guid id)
        {
            var interaction = await _repository.GetByIdAsync(id);
            if (interaction == null) return null;

            return MapToDto(interaction);
        }

        public async Task<IEnumerable<InteractionResponseDto>> GetUserInteractionsAsync(Guid userId)
        {
            var interactions = await _repository.GetUserInteractionsAsync(userId);
            return interactions.Select(MapToDto);
        }

        public async Task<bool> DeleteInteractionAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<EventStatsDto> GetEventStatsAsync(Guid eventId)
        {
            var stats = await _repository.GetEventStatsAsync(eventId);
            
            return new EventStatsDto
            {
                EventId = eventId,
                Likes = stats.TryGetValue(InteractionType.Like, out int likes) ? likes : 0,
                Dislikes = stats.TryGetValue(InteractionType.Dislike, out int dislikes) ? dislikes : 0,
                Saves = stats.TryGetValue(InteractionType.Saved, out int saves) ? saves : 0,
                Attended = stats.TryGetValue(InteractionType.Attended, out int attended) ? attended : 0
            };
        }

        public async Task<IEnumerable<EventInteractionDto>> GetUserFavoritesAsync(Guid userId)
        {
            var savedEvents = await _repository.GetUserSavedEventsAsync(userId);
            
            return savedEvents.Select(interaction => new EventInteractionDto
            {
                InteractionId = interaction.Id,
                EventId = interaction.EventId,
                EventTitle = "Event Title", // You would fetch this from Event service
                EventDate = DateTime.UtcNow, // You would fetch this from Event service
                EventImage = "image.jpg", // You would fetch this from Event service
                InteractionTime = interaction.Timestamp
            });
        }

        private static InteractionResponseDto MapToDto(EventInteraction interaction)
        {
            return new InteractionResponseDto
            {
                Id = interaction.Id,
                UserId = interaction.UserId,
                EventId = interaction.EventId,
                InteractionType = interaction.InteractionType,
                Timestamp = interaction.Timestamp,
                Metadata = interaction.Metadata
            };
        }
    }
}