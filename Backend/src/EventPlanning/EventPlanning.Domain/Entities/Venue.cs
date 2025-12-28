using System;

namespace EventPlanning.Domain.Entities
{
    public class Venue
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public Guid? SeatingLayoutId { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public Venue() { }

        // Business constructor
        public Venue(
            string name,
            string location,
            int capacity,
            Guid? seatingLayoutId,
            string contactInfo)
        {
            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            Capacity = capacity;
            SeatingLayoutId = seatingLayoutId;
            ContactInfo = contactInfo;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string name,
            string location,
            int capacity,
            Guid? seatingLayoutId,
            string contactInfo)
        {
            Name = name;
            Location = location;
            Capacity = capacity;
            SeatingLayoutId = seatingLayoutId;
            ContactInfo = contactInfo;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}