// Persistence/EventPlanningDbContext.cs
using Microsoft.EntityFrameworkCore;

public class EventPlanningDbContext : DbContext
{
    public DbSet<Event> Events { get; set; } = null!;

    public EventPlanningDbContext(DbContextOptions<EventPlanningDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Name).IsRequired().HasMaxLength(200);
            b.Property(e => e.Description).HasMaxLength(4000);
            b.Property(e => e.Category).IsRequired().HasMaxLength(100);
            b.Property(e => e.Venue).IsRequired().HasMaxLength(200);
            b.Property(e => e.StartDateUtc).IsRequired();
            b.Property(e => e.EndDateUtc).IsRequired();
            b.Ignore(e => e.DomainEvents);
        });

        base.OnModelCreating(modelBuilder);
    }
}