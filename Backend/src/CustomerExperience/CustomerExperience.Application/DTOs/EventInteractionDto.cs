using System.ComponentModel.DataAnnotations;
using CustomerExperience.Domain.Entities; 

namespace CustomerExperience.Application.DTOs
{
    public class CreateInteractionDto
    {
        [Required]
        public Guid EventId { get; set; }
        
        [Required]
        public InteractionType InteractionType { get; set; }
        
        public string? Metadata { get; set; }
    }

    public class InteractionResponseDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public InteractionType InteractionType { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Metadata { get; set; }
    }

    
    public class EventInteractionDto
    {
        public Guid InteractionId { get; set; }
        public Guid EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string EventImage { get; set; } = string.Empty;
        public DateTime InteractionTime { get; set; }
    }

    public class EventStatsDto
    {
        public Guid EventId { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Saves { get; set; }
        public int Attended { get; set; }
        public InteractionType? UserInteraction { get; set; } // null = no interaction, Like/Dislike
    }
}