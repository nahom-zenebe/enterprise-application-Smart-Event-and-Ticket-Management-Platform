using System;

namespace Shared.Domain.Payments.PaymentEvent
{
    public class PaymentCompletedDomainEvent : DomainEvent
    {
        public Guid PaymentId { get; }

        public PaymentCompletedDomainEvent(Guid paymentId)
        {
            PaymentId = paymentId;
        }
    }
}
