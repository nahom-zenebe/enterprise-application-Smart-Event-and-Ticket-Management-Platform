using System;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums; // For TicketTypeEnum and TicketStatus

namespace Ticketing.Application.DTOs
{
    public class TicketDto
    {
        public Guid TicketID { get; set; }

        /// <summary>
        /// Type of the ticket (Standard, VIP, Student, etc.)
        /// </summary>
        public TicketTypeEnum Type { get; set; }

        /// <summary>
        /// Price of the ticket
        /// </summary>
        public decimal Price { get; set; }
        public string? DiscountCode { get; set; }
        public TicketStatus Status { get; set; }

        /// <summary>
        /// ID of the reservation this ticket belongs to
        /// </summary>
        public Guid ReservationID { get; set; }
        public string? QRCode { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}