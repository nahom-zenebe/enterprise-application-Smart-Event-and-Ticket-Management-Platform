using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Entities
{
    public enum PaymentMethod
    {
        Stripe,
        Paypal
    }

    public enum PaymentStatus
    {
        Processing,
        Failed,
        Completed
    }

    public class Payment
    {
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
    }

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
            ReservationID = reservationID;
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
            DomainEvents.Add(new PaymentCompletedDomainEvent(PaymentId));
        }
        public void MarkAsFailed()
        {
            Status = PaymentStatus.Failed;
        }


}
