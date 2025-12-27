using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("events")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _eventRepository;

    public EventsController(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    [HttpPost]
    public async Task<ActionResult<EventDto>> Create([FromBody] CreateEventRequest request)
    {
        try
        {
            var @event = new Event(
                request.Name,
                request.Description,
                request.StartDateUtc,
                request.EndDateUtc,
                request.Category,
                request.Venue);

            await _eventRepository.AddAsync(@event);

            return CreatedAtAction(nameof(GetById), new { id = @event.Id }, EventDto.FromDomain(@event));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EventDto>>> List(
        [FromQuery] DateTime? dateUtc,
        [FromQuery] string? category,
        [FromQuery] string? venue)
    {
        var events = await _eventRepository.ListAsync(dateUtc, category, venue);
        return Ok(events.Select(EventDto.FromDomain).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventDto>> GetById([FromRoute] Guid id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event is null)
        {
            return NotFound();
        }

        return Ok(EventDto.FromDomain(@event));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<EventDto>> Update([FromRoute] Guid id, [FromBody] UpdateEventRequest request)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event is null)
        {
            return NotFound();
        }

        try
        {
            @event.Update(
                request.Name,
                request.Description,
                request.StartDateUtc,
                request.EndDateUtc,
                request.Category,
                request.Venue);

            await _eventRepository.UpdateAsync(@event);

            return Ok(EventDto.FromDomain(@event));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var @event = await _eventRepository.GetByIdAsync(id);
        if (@event is null)
        {
            return NotFound();
        }

        await _eventRepository.DeleteAsync(@event);
        return NoContent();
    }

    public class CreateEventRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
    }

    public class UpdateEventRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDateUtc { get; set; }
        public DateTime EndDateUtc { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Venue { get; set; } = string.Empty;
    }
}
