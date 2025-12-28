using System.Text.Json;
using EventPlanning.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking; 
namespace EventPlanning.Infrastructure.Persistence
{
    public class EventPlanningDbContext : DbContext
    {
        public EventPlanningDbContext(DbContextOptions<EventPlanningDbContext> options)
            : base(options) { }

        public DbSet<Event> Events => Set<Event>();
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<Performer> Performers => Set<Performer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.StartTime).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.EndTime).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.Capacity).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
                
                entity.Property(e => e.PerformerIds)
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<List<Guid>>(v, JsonSerializerOptions.Default) ?? new List<Guid>(),
                        new ValueComparer<List<Guid>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToList()
                        ));

                entity.HasIndex(e => e.EventId);
            });

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

            modelBuilder.Entity<Performer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Bio).HasMaxLength(2000);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.PerformanceType).IsRequired().HasMaxLength(100);
                
                entity.Property(e => e.SocialLinks)
                    .HasColumnType("jsonb")
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                        v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, JsonSerializerOptions.Default) 
                            ?? new Dictionary<string, string>(),
                        new ValueComparer<Dictionary<string, string>>(
                            (c1, c2) => c1.SequenceEqual(c2),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c.ToDictionary(x => x.Key, x => x.Value)
                        ));
                
                entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.UpdatedAt).HasColumnType("timestamp with time zone");
            });
        }
    }
}