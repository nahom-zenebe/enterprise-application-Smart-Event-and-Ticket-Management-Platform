using Microsoft.AspNetCore.Mvc;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("events/{eventId}/media")]
    public class EventMediaController : ControllerBase
    {
        // MOCK storage (acceptable for demo)
        private static readonly List<EventMediaDto> MediaStore = new();

        [HttpPost]
        public IActionResult UploadMedia(int eventId, [FromBody] EventMediaDto dto)
        {
            dto.Id = MediaStore.Count + 1;
            dto.EventId = eventId;
            MediaStore.Add(dto);

            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetMedia(int eventId)
        {
            var media = MediaStore.Where(m => m.EventId == eventId);
            return Ok(media);
        }
    }

    [ApiController]
    [Route("media")]
    public class MediaController : ControllerBase
    {
        private static readonly List<EventMediaDto> MediaStore = typeof(EventMediaController)
            .GetField("MediaStore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?
            .GetValue(null) as List<EventMediaDto> ?? new();

        [HttpPut("{id}")]
        public IActionResult UpdateMedia(int id, [FromBody] EventMediaDto dto)
        {
            var media = MediaStore.FirstOrDefault(m => m.Id == id);
            if (media == null) return NotFound();

            media.Title = dto.Title;
            media.Url = dto.Url;
            media.Type = dto.Type;

            return Ok(media);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMedia(int id)
        {
            var media = MediaStore.FirstOrDefault(m => m.Id == id);
            if (media == null) return NotFound();

            MediaStore.Remove(media);
            return NoContent();
        }
    }

    public class EventMediaDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Type { get; set; } = ""; // image, video, pdf
    }
}
