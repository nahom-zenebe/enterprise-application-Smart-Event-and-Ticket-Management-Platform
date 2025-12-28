using System;
using System.ComponentModel.DataAnnotations;

namespace EventPlanning.Application.DTOs
{
    public class CreateVenueDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
        
        public Guid? SeatingLayoutId { get; set; }
        
        [Required]
        public string ContactInfo { get; set; } = string.Empty;
    }

    public class UpdateVenueDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
        
        public Guid? SeatingLayoutId { get; set; }
        
        [Required]
        public string ContactInfo { get; set; } = string.Empty;
    }

    public class VenueResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public Guid? SeatingLayoutId { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}