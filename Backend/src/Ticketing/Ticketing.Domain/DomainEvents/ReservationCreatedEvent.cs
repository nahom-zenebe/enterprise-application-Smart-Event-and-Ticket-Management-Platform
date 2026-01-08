using System;

namespace Ticketing.Domain.DomainEvents
{
    public class ReservationCreatedEvent : IDomainEvent
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime OccurredAt { get; } = DateTime.UtcNow;
        public string EventType => "ReservationCreated";
        
        public Guid ReservationId { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }
}