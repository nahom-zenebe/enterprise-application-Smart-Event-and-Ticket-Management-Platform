using System;

public abstract class DomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
