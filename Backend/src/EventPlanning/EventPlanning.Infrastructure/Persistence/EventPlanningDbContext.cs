using System.Text.Json;
using EventPlanning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Infrastructure.Persistence
{
    public class EventPlanningDbContext : DbContext
    {
        public EventPlanningDbContext(DbContextOptions<EventPlanningDbContext> options)
            : base(options) { }

        // Events
        public DbSet<Event> Events => Set<Event>();
        
        // Session and Venue
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Venue> Venues => Set<Venue>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Event entity
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(4000);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Venue).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StartDateUtc).IsRequired();
                entity.Property(e => e.EndDateUtc).IsRequired();
                entity.Ignore(e => e.DomainEvents);
            });

            // Configure Session entity
            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StartTime).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.EndTime).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.Capacity).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
                
                // Configure PerformerIds as JSON array
                entity.Property(e => e.PerformerIds)
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions)null) ?? new List<Guid>());

                entity.HasIndex(e => e.EventId);
            });

            // Configure Venue entity
            modelBuilder.Entity<Venue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Capacity).IsRequired();
                entity.Property(e => e.ContactInfo).IsRequired().HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
            });
        }
    }
}