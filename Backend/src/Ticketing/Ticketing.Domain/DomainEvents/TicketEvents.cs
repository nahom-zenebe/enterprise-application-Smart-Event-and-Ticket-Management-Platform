using System;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.DomainEvents
{
    public class TicketCreated : DomainEvent
    {
        public Ticket Ticket { get; }

        public TicketCreated(Ticket ticket)
        {
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }

    public class TicketConfirmed : DomainEvent
    {
        public Ticket Ticket { get; }

        public TicketConfirmed(Ticket ticket)
        {
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }

    public class TicketCancelled : DomainEvent
    {
        public Ticket Ticket { get; }

        public TicketCancelled(Ticket ticket)
        {
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }

    public class TicketCheckedIn : DomainEvent
    {
        public Ticket Ticket { get; }

        public TicketCheckedIn(Ticket ticket)
        {
            Ticket = ticket ?? throw new ArgumentNullException(nameof(ticket));
        }
    }
}
