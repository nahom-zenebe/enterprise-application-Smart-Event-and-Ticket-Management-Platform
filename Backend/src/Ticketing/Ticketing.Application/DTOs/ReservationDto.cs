using System;
using System.Collections.Generic;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.DTOs
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime ReservedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public List<TicketDto> Tickets { get; set; } = new();
    }
}