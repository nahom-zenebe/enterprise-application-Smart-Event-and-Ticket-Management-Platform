using System;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.DomainEvents{
    public class ReservationCreatedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Reservation Reservation { get; }

        public ReservationCreatedDomainEvent(Reservation reservation)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "ReservationCreated";
            Reservation = reservation;
        }
    }

    public class ReservationCancelledDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Reservation Reservation { get; }

        public ReservationCancelledDomainEvent(Reservation reservation)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "ReservationCancelled";
            Reservation = reservation;
        }
    }
    
    public class ReservationConfirmedDomainEvent : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Reservation Reservation { get; }

        public ReservationConfirmedDomainEvent(Reservation reservation)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "ReservationConfirmed";
            Reservation = reservation;
        }
    }
}
