// DomainEvents/EventCreated.cs
using System;

public class EventCreated : DomainEvent
{
    public Event Event { get; }

    public EventCreated(Event @event)
    {
        Event = @event ?? throw new ArgumentNullException(nameof(@event));
    }
}