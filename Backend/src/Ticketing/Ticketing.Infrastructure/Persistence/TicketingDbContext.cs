using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Persistence
{
    public class TicketingDbContext : DbContext
    {
        public TicketingDbContext(DbContextOptions<TicketingDbContext> options)
            : base(options) { }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<OutboxEvent> OutboxEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure OutboxEvent entity
            modelBuilder.Entity<OutboxEvent>(entity =>
            {
                entity.Property(e => e.MaxRetries).HasDefaultValue(3);
                entity.Property(e => e.RetryCount).HasDefaultValue(0);
                entity.Property(e => e.LastError).IsRequired(false);
            });
        }
    }
}
