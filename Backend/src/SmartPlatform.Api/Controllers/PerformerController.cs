using EventPlanning.Application.DTOs;
using EventPlanning.Application.Interfaces;
using EventPlanning.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformersController : ControllerBase
    {
        private readonly IPerformerRepository _performerRepository;

        public PerformersController(IPerformerRepository performerRepository)
        {
            _performerRepository = performerRepository;
        }

        [HttpPost]
        public async Task<ActionResult<PerformerDto>> CreatePerformer([FromBody] CreatePerformerDto createDto)
        {
            var performer = new Performer
            {
                Name = createDto.Name,
                Bio = createDto.Bio,
                ImageUrl = createDto.ImageUrl,
                PerformanceType = createDto.PerformanceType,
                SocialLinks = createDto.SocialLinks
            };

            var createdPerformer = await _performerRepository.AddAsync(performer);
            
            return CreatedAtAction(nameof(GetPerformer), new { id = createdPerformer.Id }, MapToDto(createdPerformer));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PerformerDto>>> GetAllPerformers()
        {
            var performers = await _performerRepository.GetAllAsync();
            var performerDtos = performers.Select(MapToDto).ToList();
            
            return Ok(performerDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PerformerDto>> GetPerformer(Guid id)
        {
            var performer = await _performerRepository.GetByIdAsync(id);
            
            if (performer == null)
                return NotFound();
            
            return Ok(MapToDto(performer));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PerformerDto>> UpdatePerformer(Guid id, [FromBody] UpdatePerformerDto updateDto)
        {
            var performer = await _performerRepository.GetByIdAsync(id);
            
            if (performer == null)
                return NotFound();

            performer.Name = updateDto.Name;
            performer.Bio = updateDto.Bio;
            performer.ImageUrl = updateDto.ImageUrl;
            performer.PerformanceType = updateDto.PerformanceType;
            performer.SocialLinks = updateDto.SocialLinks;
            performer.UpdatedAt = DateTime.UtcNow;

            await _performerRepository.UpdateAsync(performer);
            
            return Ok(MapToDto(performer));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerformer(Guid id)
        {
            var performer = await _performerRepository.GetByIdAsync(id);
            
            if (performer == null)
                return NotFound();

            await _performerRepository.DeleteAsync(performer);
            
            return NoContent();
        }

        private static PerformerDto MapToDto(Performer performer)
        {
            return new PerformerDto
            {
                Id = performer.Id,
                Name = performer.Name,
                Bio = performer.Bio,
                ImageUrl = performer.ImageUrl,
                PerformanceType = performer.PerformanceType,
                SocialLinks = performer.SocialLinks,
                CreatedAt = performer.CreatedAt,
                UpdatedAt = performer.UpdatedAt
            };
        }
    }
}