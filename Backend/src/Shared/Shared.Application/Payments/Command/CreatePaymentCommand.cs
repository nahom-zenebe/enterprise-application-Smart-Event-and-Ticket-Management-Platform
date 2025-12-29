using Shared.Domain.Payments.Enums;

namespace Shared.Application.Payments.Commands.CreatePayment
{
    public record CreatePaymentCommand(
        int UserId,
        decimal Amount,
        PaymentMethod Method
    );
}
