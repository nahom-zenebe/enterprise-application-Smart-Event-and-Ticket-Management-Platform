using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ticketing.Domain.Enums; // <-- Important: Use the enum from Enums folder

namespace Ticketing.Domain.Entities
{
    public enum TicketStatus
    {
        Reserved,
        Confirmed,
        Cancelled,
        CheckedIn
    }

    // REMOVED: public enum TicketType { ... }
    // We now exclusively use TicketTypeEnum from Ticketing.Domain.Enums

    public class Ticket
    {
        [Key]
        [Required]
        public Guid TicketID { get; set; } = Guid.NewGuid();

        [Required]
        public TicketTypeEnum Type { get; set; } = TicketTypeEnum.Standard; // or Regular, VIP, etc.

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? DiscountCode { get; set; } // Nullable → fixes CS8618

        [Required]
        public TicketStatus Status { get; private set; } = TicketStatus.Reserved;

        [Required]
        public Guid ReservationID { get; set; }

        public string? QRCode { get; set; } // Nullable → fixes CS8618 (generated later)

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Constructor for manual creation
        public Ticket(
            Guid ticketID,
            TicketTypeEnum type,
            decimal price,
            string? qrCode,
            string? discountCode,
            Guid reservationID,
            TicketStatus status,
            DateTime createdAt,
            DateTime updatedAt
        )
        {
            TicketID = ticketID != Guid.Empty ? ticketID : Guid.NewGuid();
            Type = type;
            Price = price;
            QRCode = qrCode;
            DiscountCode = discountCode;
            ReservationID = reservationID;
            Status = status;
            CreatedAt = createdAt != default ? createdAt : DateTime.UtcNow;
            UpdatedAt = updatedAt != default ? updatedAt : DateTime.UtcNow;
        }

        // Parameterless constructor required by Entity Framework
        protected Ticket() { }

        public void Confirm()
        {
            if (Status != TicketStatus.Reserved)
                throw new InvalidOperationException("Only reserved tickets can be confirmed.");

            Status = TicketStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status == TicketStatus.CheckedIn)
                throw new InvalidOperationException("Checked-in tickets cannot be cancelled.");

            Status = TicketStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        public void CheckIn()
        {
            if (Status != TicketStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed tickets can be checked-in.");

            Status = TicketStatus.CheckedIn;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Update(
            TicketTypeEnum? type = null,
            decimal? price = null,
            string? discountCode = null,
            string? qrCode = null
        )
        {
            if (type.HasValue)
                Type = type.Value;

            if (price.HasValue)
                Price = price.Value;

            if (discountCode != null) // Covers both null and empty/whitespace
                DiscountCode = discountCode;

            if (qrCode != null)
                QRCode = qrCode;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}