using System;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.DomainEvents
{
    public class TicketCreated : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Ticket Ticket { get; }

        public TicketCreated(Ticket ticket)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "TicketCreated";
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }

    public class TicketConfirmed : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Ticket Ticket { get; }

        public TicketConfirmed(Ticket ticket)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "TicketConfirmed";
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }

    public class TicketCancelled : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Ticket Ticket { get; }

        public TicketCancelled(Ticket ticket)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "TicketCancelled";
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }

    public class TicketCheckedIn : IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredAt { get; }
        public string EventType { get; }
        public Ticket Ticket { get; }

        public TicketCheckedIn(Ticket ticket)
        {
            Id = Guid.NewGuid();
            OccurredAt = DateTime.UtcNow;
            EventType = "TicketCheckedIn";
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }
}
