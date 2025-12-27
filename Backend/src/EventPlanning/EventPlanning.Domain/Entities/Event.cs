// Entities/Event.cs
using System;

public class Event
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime StartDateUtc { get; private set; }
    public DateTime EndDateUtc { get; private set; }
    public string Category { get; private set; }
    public string Venue { get; private set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents;

    private Event() { }

    public Event(
        string name,
        string? description,
        DateTime startDateUtc,
        DateTime endDateUtc,
        string category,
        string venue)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Event name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required.", nameof(category));
        if (string.IsNullOrWhiteSpace(venue)) throw new ArgumentException("Venue is required.", nameof(venue));
        if (endDateUtc < startDateUtc) throw new ArgumentException("End date must be greater than or equal to start date.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        StartDateUtc = DateTime.SpecifyKind(startDateUtc, DateTimeKind.Utc);
        EndDateUtc = DateTime.SpecifyKind(endDateUtc, DateTimeKind.Utc);
        Category = category;
        Venue = venue;

        _domainEvents.Add(new EventCreated(this));
    }

    public void Update(
        string name,
        string? description,
        DateTime startDateUtc,
        DateTime endDateUtc,
        string category,
        string venue)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Event name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required.", nameof(category));
        if (string.IsNullOrWhiteSpace(venue)) throw new ArgumentException("Venue is required.", nameof(venue));
        if (endDateUtc < startDateUtc) throw new ArgumentException("End date must be greater than or equal to start date.");

        Name = name;
        Description = description;
        StartDateUtc = DateTime.SpecifyKind(startDateUtc, DateTimeKind.Utc);
        EndDateUtc = DateTime.SpecifyKind(endDateUtc, DateTimeKind.Utc);
        Category = category;
        Venue = venue;
    }
}