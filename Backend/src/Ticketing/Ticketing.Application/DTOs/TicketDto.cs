using System;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.DTOs
{
    public class TicketDto
    {
        public Guid TicketID { get; set; }
        public TicketType Type { get; set; }
        public decimal Price { get; set; }
        public string DiscountCode { get; set; }
        public TicketStatus Status { get; set; }
        public Guid ReservationID { get; set; }
        public string QRCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
