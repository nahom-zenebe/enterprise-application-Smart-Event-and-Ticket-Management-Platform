using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Shared.Domain.Payments.Enums;
using Shared.Domain.Payments.PaymentEvent;

namespace Shared.Domain.Payments.Entities
{
    public class Payment
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

        [Key]
        [Required]
        public Guid PaymentId { get; set; } = Guid.NewGuid();
        
        [Required]
        public int TicketId{get;set;}

        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Stripe;

        public PaymentStatus Status { get; set; } = PaymentStatus.Processing;

        public DateTime PaidAt { get; set; } = DateTime.UtcNow;

        protected Payment() { }

        public Payment(
            Guid paymentId,
            int ticketId,
            int userId,
            decimal  amount,
            PaymentMethod paymentMethod,
            PaymentStatus status,
            DateTime paidAt
        )
        {
            PaymentId = paymentId != Guid.Empty ? paymentId : Guid.NewGuid();
            TicketId=ticketId;
            UserId = userId;
            Amount= amount;
            PaymentMethod = paymentMethod;
            Status = status;
            PaidAt = paidAt != default ? paidAt : DateTime.UtcNow;
        }

        public void MarkAsCompleted()
        {
            if (Status == PaymentStatus.Completed)
                throw new InvalidOperationException("Payment already completed");

            Status = PaymentStatus.Completed;
            PaidAt = DateTime.UtcNow;

            // Raise Domain Event
            _domainEvents.Add(new PaymentCompletedDomainEvent(PaymentId));
        }

        public void MarkAsFailed()
        {
            Status = PaymentStatus.Failed;
        }
    }
}
