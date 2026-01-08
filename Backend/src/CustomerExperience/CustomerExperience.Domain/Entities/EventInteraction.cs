using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerExperience.Domain.Entities
{
    public enum InteractionType
    {
        Like,
        Dislike,
        Saved,
        Attended,
        Viewed,
        Shared
    }

    [Table("EventInteractions")]
    public class EventInteraction
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid EventId { get; set; }
        
        [Required]
        public InteractionType InteractionType { get; set; }
        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        
        // Additional metadata
        public string? Metadata { get; set; } // JSON for additional data
        public string? UserAgent { get; set; }
        public string? IpAddress { get; set; }
    }
}