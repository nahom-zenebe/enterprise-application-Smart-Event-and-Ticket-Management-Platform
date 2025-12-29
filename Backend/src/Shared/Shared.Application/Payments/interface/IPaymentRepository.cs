using System;
using System.Threading.Tasks;
using Shared.Domain.Payments.Entities;

namespace Shared.Application.Payments.interface
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment?> GetByIdAsync(Guid paymentId);
        Task SaveChangesAsync();
    }
}
