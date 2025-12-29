using Microsoft.EntityFrameworkCore;
using Shared.Domain.Payments.Entities;

namespace Shared.Infrastructure.Payments.Persistence
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments => Set<Payment>();

        public PaymentDbContext(DbContextOptions options)
            : base(options) { }
    }
}
