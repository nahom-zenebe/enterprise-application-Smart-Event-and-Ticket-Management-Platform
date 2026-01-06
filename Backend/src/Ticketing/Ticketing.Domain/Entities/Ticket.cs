using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.Domain.Entities
{
    public enum TicketStatus
    {
        Reserved,
        Confirmed,
        Cancelled,
        CheckedIn
    }

    public enum TicketType
    {
        Regular,
        VIP,
        Student
    }

    public class Ticket
    {
        [Key]
        [Required]
        public Guid TicketID { get; set; } = Guid.NewGuid();

        [Required]
        public TicketType Type { get; set; } = TicketType.Regular;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string? DiscountCode { get; set; } // optional

        [Required]
        public TicketStatus Status { get; private set; } = TicketStatus.Reserved;

        [Required]
        public Guid ReservationID { get; set; }

        public string? QRCode { get; set; } // could store base64 or barcode string

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Constructor for manual creation
        public Ticket(
            Guid ticketID,
            TicketType type,
            decimal price,
            string qrCode,
            string discountCode,
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

        // Parameterless constructor for EF
        protected Ticket() { }

        // Confirm the ticket
        public void Confirm()
        {
            if (Status != TicketStatus.Reserved)
                throw new InvalidOperationException("Only reserved tickets can be confirmed.");

            Status = TicketStatus.Confirmed;
            UpdatedAt = DateTime.UtcNow;
        }

        // Cancel the ticket
        public void Cancel()
        {
            if (Status == TicketStatus.CheckedIn)
                throw new InvalidOperationException("Checked-in tickets cannot be cancelled.");

            Status = TicketStatus.Cancelled;
            UpdatedAt = DateTime.UtcNow;
        }

        // Check-in the ticket
        public void CheckIn()
        {
            if (Status != TicketStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed tickets can be checked-in.");

            Status = TicketStatus.CheckedIn;
            UpdatedAt = DateTime.UtcNow;
        }

        // Update ticket properties
        public void Update(
            TicketType? type = null,
            decimal? price = null,
            string? discountCode = null,
            string? qrCode = null
        )
        {
            if (type.HasValue)
                Type = type.Value;

            if (price.HasValue)
                Price = price.Value;

            if (!string.IsNullOrWhiteSpace(discountCode))
                DiscountCode = discountCode;

            if (!string.IsNullOrWhiteSpace(qrCode))
                QRCode = qrCode;

            UpdatedAt = DateTime.UtcNow;
        }
    }
}
