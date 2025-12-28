using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EventPlanning.Application.DTOs
{
    public class CreateSessionDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        public List<Guid> PerformerIds { get; set; } = new List<Guid>(); // Changed from string to Guid
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
        
        public Guid? SeatingMapId { get; set; }
    }

    public class UpdateSessionDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public DateTime StartTime { get; set; }
        
        [Required]
        public DateTime EndTime { get; set; }
        
        public List<Guid> PerformerIds { get; set; } = new List<Guid>(); // Changed from string to Guid
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
        
        public Guid? SeatingMapId { get; set; }
    }

    public class SessionResponseDto
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Guid> PerformerIds { get; set; } = new List<Guid>(); // Changed from string to Guid
        public int Capacity { get; set; }
        public Guid? SeatingMapId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}