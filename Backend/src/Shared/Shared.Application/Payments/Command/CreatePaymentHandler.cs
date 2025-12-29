using System.Threading.Tasks;
using Shared.Application.Payments.Interfaces;
using Shared.Domain.Payments.Entities;
using Shared.Domain.Payments.Repositories;

namespace Shared.Application.Payments.Commands.CreatePayment
{
    public class CreatePaymentHandler
    {
        private readonly IPaymentRepository _repository;
        private readonly IPaymentGateway _gateway;

        public CreatePaymentHandler(
            IPaymentRepository repository,
            IPaymentGateway gateway)
        {
            _repository = repository;
            _gateway = gateway;
        }

        public async Task Handle(CreatePaymentCommand command)
        {
            var payment = new Payment(
                command.UserId,
                command.Amount,
                command.Method
            );

            var success = await _gateway.ProcessAsync(command.Amount);

            if (success)
                payment.MarkAsCompleted();
            else
                payment.MarkAsFailed();

            await _repository.AddAsync(payment);
            await _repository.SaveChangesAsync();
        }
    }
}
