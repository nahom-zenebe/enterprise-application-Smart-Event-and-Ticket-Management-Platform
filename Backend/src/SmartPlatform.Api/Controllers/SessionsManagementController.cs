using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using EventPlanning.Application.Interfaces;
using EventPlanning.Application.DTOs;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/sessions")]
    public class SessionsManagementController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionsManagementController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        // PUT /api/sessions/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SessionResponseDto>> UpdateSession(
            Guid id, 
            [FromBody] UpdateSessionDto updateSessionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if start time is before end time
            if (updateSessionDto.StartTime >= updateSessionDto.EndTime)
                return BadRequest("Start time must be before end time");

            var existingSession = await _sessionRepository.GetByIdAsync(id);
            if (existingSession == null)
                return NotFound($"Session with ID {id} not found");

            existingSession.Update(
                updateSessionDto.Name,
                updateSessionDto.StartTime,
                updateSessionDto.EndTime,
                updateSessionDto.PerformerIds,
                updateSessionDto.Capacity,
                updateSessionDto.SeatingMapId);

            var updatedSession = await _sessionRepository.UpdateAsync(existingSession);

            var response = new SessionResponseDto
            {
                Id = updatedSession.Id,
                EventId = updatedSession.EventId,
                Name = updatedSession.Name,
                StartTime = updatedSession.StartTime,
                EndTime = updatedSession.EndTime,
                PerformerIds = updatedSession.PerformerIds,
                Capacity = updatedSession.Capacity,
                SeatingMapId = updatedSession.SeatingMapId,
                CreatedAt = updatedSession.CreatedAt,
                UpdatedAt = updatedSession.UpdatedAt
            };

            return Ok(response);
        }

        // DELETE /api/sessions/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            var deleted = await _sessionRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Session with ID {id} not found");

            return NoContent();
        }
    }
}