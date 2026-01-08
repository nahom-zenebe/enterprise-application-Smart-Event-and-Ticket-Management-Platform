using Microsoft.EntityFrameworkCore;
using CustomerExperience.Domain.Entities;
namespace CustomerExperience.Infrastructure.Persistence
{
    public class CustomerExperienceDbContext : DbContext
    {
        public CustomerExperienceDbContext(DbContextOptions<CustomerExperienceDbContext> options)
            : base(options) { }

        public DbSet<EventInteraction> EventInteractions => Set<EventInteraction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure EventInteraction entity
            modelBuilder.Entity<EventInteraction>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Indexes for faster queries
                entity.HasIndex(e => new { e.UserId, e.EventId, e.InteractionType })
                      .IsUnique(); // Prevent duplicate interactions of same type
                
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.EventId);
                entity.HasIndex(e => e.InteractionType);
                entity.HasIndex(e => e.Timestamp);
                
                // Configure enum as string for readability
                entity.Property(e => e.InteractionType)
                      .HasConversion<string>()
                      .HasMaxLength(20);
                
                // Configure JSON metadata
                entity.Property(e => e.Metadata)
                      .HasColumnType("jsonb");
                
                // Configure timestamps
                entity.Property(e => e.Timestamp)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}