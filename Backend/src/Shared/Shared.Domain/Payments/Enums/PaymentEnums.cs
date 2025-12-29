namespace Shared.Domain.Payments.Enums
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
}

