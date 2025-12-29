using Microsoft.EntityFrameworkCore;
using Shared.Domain.Payments.Entities;

namespace Shared.Infrastructure.Payments.Persistence
{
    public class PaymentDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; } = null!;

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);
                entity.Property(e => e.TicketId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Amount).IsRequired().HasPrecision(18, 2);
                entity.Property(e => e.PaymentMethod).IsRequired();
                entity.Property(e => e.Status).IsRequired();
                entity.Property(e => e.PaidAt).IsRequired();
                
                // Ignore domain events as they're not stored in the database
                entity.Ignore(e => e.DomainEvents);
            });
        }
    }
}
