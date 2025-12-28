using System;
using System.Collections.Generic;

namespace EventPlanning.Domain.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Guid> PerformerIds { get; set; } = new(); // Should be List<Guid>
        public int Capacity { get; set; }
        public Guid? SeatingMapId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parameterless constructor for EF Core
        public Session() { }

        // Business constructor
        public Session(
            Guid eventId,
            string name,
            DateTime startTime,
            DateTime endTime,
            List<Guid> performerIds,
            int capacity,
            Guid? seatingMapId)
        {
            Id = Guid.NewGuid();
            EventId = eventId;
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            PerformerIds = performerIds ?? new List<Guid>();
            Capacity = capacity;
            SeatingMapId = seatingMapId;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string name,
            DateTime startTime,
            DateTime endTime,
            List<Guid> performerIds,
            int capacity,
            Guid? seatingMapId)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            PerformerIds = performerIds ?? new List<Guid>();
            Capacity = capacity;
            SeatingMapId = seatingMapId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}