using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlanning.Application.Interfaces;
using EventPlanning.Application.DTOs;
using EventPlanning.Domain.Entities;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/events/{eventId}/sessions")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionsController(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        // POST /api/events/{eventId}/sessions
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SessionResponseDto>> CreateSession(
            Guid eventId, 
            [FromBody] CreateSessionDto createSessionDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Check if start time is before end time
            if (createSessionDto.StartTime >= createSessionDto.EndTime)
                return BadRequest("Start time must be before end time");

            var session = new Session(
                eventId,
                createSessionDto.Name,
                createSessionDto.StartTime,
                createSessionDto.EndTime,
                createSessionDto.PerformerIds,
                createSessionDto.Capacity,
                createSessionDto.SeatingMapId);

            var createdSession = await _sessionRepository.CreateAsync(session);

            var response = new SessionResponseDto
            {
                Id = createdSession.Id,
                EventId = createdSession.EventId,
                Name = createdSession.Name,
                StartTime = createdSession.StartTime,
                EndTime = createdSession.EndTime,
                PerformerIds = createdSession.PerformerIds,
                Capacity = createdSession.Capacity,
                SeatingMapId = createdSession.SeatingMapId,
                CreatedAt = createdSession.CreatedAt,
                UpdatedAt = createdSession.UpdatedAt
            };

            return CreatedAtAction(
                nameof(GetSessions), 
                new { eventId = eventId }, 
                response);
        }

        // GET /api/events/{eventId}/sessions
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<SessionResponseDto>>> GetSessions(Guid eventId)
        {
            var sessions = await _sessionRepository.GetByEventIdAsync(eventId);
            
            var response = sessions.Select(session => new SessionResponseDto
            {
                Id = session.Id,
                EventId = session.EventId,
                Name = session.Name,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                PerformerIds = session.PerformerIds,
                Capacity = session.Capacity,
                SeatingMapId = session.SeatingMapId,
                CreatedAt = session.CreatedAt,
                UpdatedAt = session.UpdatedAt
            }).ToList();

            return Ok(response);
        }
    }
}