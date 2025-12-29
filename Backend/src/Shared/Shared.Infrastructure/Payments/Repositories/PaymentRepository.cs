using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Payments.Interfaces;
using Shared.Domain.Payments.Entities;
using Shared.Infrastructure.Payments.Persistence;

namespace Shared.Infrastructure.Payments.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PaymentDbContext _context;

        public PaymentRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
            => await _context.Payments.AddAsync(payment);

        public async Task<Payment?> GetByIdAsync(Guid id)
            => await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
