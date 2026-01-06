using System;
using Ticketing.Domain.Entities;

namespace Ticketing.Domain.DomainEvents{
    public class ReservationCreatedDomainEvent : IDomainEvent
    {
        public Reservation Reservation { get; }

        public ReservationCreatedDomainEvent(Reservation reservation)
        {
            Reservation = reservation;
        }
    }

    public class ReservationCancelledDomainEvent : IDomainEvent
    {
        public Reservation Reservation { get; }

        public ReservationCancelledDomainEvent(Reservation reservation)
        {
            Reservation = reservation;
        }
    }
    public class ReservationConfirmedDomainEvent : IDomainEvent
    {
        public Reservation Reservation { get; }

        public ReservationConfirmedDomainEvent(Reservation reservation)
        {
            Reservation = reservation;
        }
    }
    
}
