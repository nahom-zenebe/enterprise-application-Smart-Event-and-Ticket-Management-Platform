
using System;

public class EventDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartDateUtc { get; set; }
    public DateTime EndDateUtc { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Venue { get; set; } = string.Empty;

    public static EventDto FromDomain(Event @event) => new()
    {
        Id = @event.Id,
        Name = @event.Name,
        Description = @event.Description,
        StartDateUtc = @event.StartDateUtc,
        EndDateUtc = @event.EndDateUtc,
        Category = @event.Category,
        Venue = @event.Venue
    };
}
