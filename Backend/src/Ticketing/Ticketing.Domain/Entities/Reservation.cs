using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ticketing.Domain.Common;

namespace Ticketing.Domain.Entities
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Reservation : AggregateRoot
    {
        [Key]
        public Guid Id { get; private set; }

        [Required]
        public Guid UserId { get; private set; }

        [Required]
        public Guid EventId { get; private set; }

        public ReservationStatus Status { get; private set; }

        [Required]
        public DateTime ReservedAt { get; private set; }

        [Required]
        public DateTime ExpiresAt { get; private set; }

        private readonly List<Ticket> _tickets = new();
        public IReadOnlyCollection<Ticket> Tickets => _tickets.AsReadOnly();

        // EF Core constructor
        private Reservation() { }

        // Domain constructor
        public Reservation(Guid userId, Guid eventId, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            EventId = eventId;
            ReservedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
            Status = ReservationStatus.Pending;
        }

        // Domain behavior
        public void AddTicket(Ticket ticket)
        {
            if (Status != ReservationStatus.Pending)
                throw new InvalidOperationException("Cannot add tickets unless reservation is pending.");

            _tickets.Add(ticket);
        }

        public void Confirm()
        {
            if (!_tickets.Any())
                throw new InvalidOperationException("Cannot confirm reservation without tickets.");

            Status = ReservationStatus.Confirmed;
        }

        public void Cancel()
        {
            Status = ReservationStatus.Cancelled;
        }
    }
}
