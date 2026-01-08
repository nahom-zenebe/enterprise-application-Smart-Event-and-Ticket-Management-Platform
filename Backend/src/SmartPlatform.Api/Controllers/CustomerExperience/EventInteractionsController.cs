//EventInteractionsController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using CustomerExperience.Application.Interfaces;
using CustomerExperience.Application.DTOs;
using CustomerExperience.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace SmartPlatform.Api.Controllers.CustomerExperience
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class EventInteractionsController : ControllerBase
    {
        private readonly IEventInteractionRepository _interactionRepository;
        private readonly ILogger<EventInteractionsController> _logger;

        public EventInteractionsController(
            IEventInteractionRepository interactionRepository,
            ILogger<EventInteractionsController> logger)
        {
            _interactionRepository = interactionRepository;
            _logger = logger;
        }

        // Helper method to get current user ID from JWT token
private Guid GetCurrentUserId()
{
    try 
    {
        _logger.LogInformation("Available claims: {Claims}", 
            User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList());

        // Try different possible claim types
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("sub")?.Value
                         ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value
                         ?? User.FindFirst("uid")?.Value
                         ?? User.FindFirst("user_id")?.Value;

        _logger.LogInformation("Found user ID claim: {ClaimValue}", userIdClaim ?? "null");

        if (string.IsNullOrEmpty(userIdClaim))
        {
            _logger.LogWarning("No user ID claim found in the token. Available claims: {Claims}", 
                User.Claims.Select(c => c.Type).ToList());
            throw new UnauthorizedAccessException("No user ID claim found in the token");
        }

        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            _logger.LogInformation("Successfully parsed user ID: {UserId}", userId);
            return userId;
        }

        _logger.LogWarning("Failed to parse user ID from token. Claim value: {ClaimValue}", userIdClaim);
        throw new UnauthorizedAccessException("Invalid user ID format in token");
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting current user ID. User authenticated: {IsAuthenticated}, Claims count: {ClaimsCount}", 
            User.Identity?.IsAuthenticated, User.Claims?.Count() ?? 0);
        throw;
    }
}
[HttpGet("debug/token")]
public IActionResult DebugToken()
{
    var claims = User.Claims
        .ToDictionary(c => c.Type, c => c.Value);
    
    return Ok(new {
        IsAuthenticated = User.Identity?.IsAuthenticated,
        AuthenticationType = User.Identity?.AuthenticationType,
        Claims = claims
    });
}
        // POST /api/events/{eventId}/like
        [HttpPost("events/{eventId}/like")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LikeResponseDto>> LikeEvent(Guid eventId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Check if user already liked this event
                var existingLike = await _interactionRepository
                    .GetUserEventInteractionAsync(userId, eventId, InteractionType.Like);
                
                if (existingLike != null)
                {
                    // User already liked - toggle off (unlike)
                    await _interactionRepository.RemoveInteractionAsync(userId, eventId, InteractionType.Like);
                    
                    _logger.LogInformation("User {UserId} unliked event {EventId}", userId, eventId);
                    
                    return Ok(new LikeResponseDto
                    {
                        Liked = false,
                        Message = "Event unliked",
                        Interaction = null,
                        Likes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Like),
                        Dislikes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Dislike)
                    });
                }
                
                // Remove any existing dislike first
                await _interactionRepository.RemoveInteractionAsync(userId, eventId, InteractionType.Dislike);
                
                // Create new like
                var interaction = new EventInteraction
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EventId = eventId,
                    InteractionType = InteractionType.Like,
                    Timestamp = DateTime.UtcNow,
                    UserAgent = Request.Headers["User-Agent"].ToString(),
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                var createdInteraction = await _interactionRepository.CreateAsync(interaction);
                
                _logger.LogInformation("User {UserId} liked event {EventId}", userId, eventId);
                
                return Ok(new LikeResponseDto
                {
                    Liked = true,
                    Message = "Event liked",
                    Interaction = new InteractionResponseDto
                    {
                        Id = createdInteraction.Id,
                        UserId = createdInteraction.UserId,
                        EventId = createdInteraction.EventId,
                        InteractionType = createdInteraction.InteractionType,
                        Timestamp = createdInteraction.Timestamp,
                        Metadata = createdInteraction.Metadata
                    },
                    Likes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Like),
                    Dislikes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Dislike)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error liking event {EventId}", eventId);
                return StatusCode(500, new { 
                    message = "An error occurred while liking the event", 
                    error = ex.Message 
                });
            }
        }

        // POST /api/events/{eventId}/dislike
        [HttpPost("events/{eventId}/dislike")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DislikeResponseDto>> DislikeEvent(Guid eventId)
        {
            try
            {
                var userId = GetCurrentUserId();
                
                // Check if user already disliked this event
                var existingDislike = await _interactionRepository
                    .GetUserEventInteractionAsync(userId, eventId, InteractionType.Dislike);
                
                if (existingDislike != null)
                {
                    // User already disliked - toggle off (remove dislike)
                    await _interactionRepository.RemoveInteractionAsync(userId, eventId, InteractionType.Dislike);
                    
                    _logger.LogInformation("User {UserId} removed dislike from event {EventId}", userId, eventId);
                    
                    return Ok(new DislikeResponseDto
                    {
                        Disliked = false,
                        Message = "Dislike removed",
                        Interaction = null,
                        Likes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Like),
                        Dislikes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Dislike)
                    });
                }
                
                // Remove any existing like first
                await _interactionRepository.RemoveInteractionAsync(userId, eventId, InteractionType.Like);
                
                var interaction = new EventInteraction
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    EventId = eventId,
                    InteractionType = InteractionType.Dislike,
                    Timestamp = DateTime.UtcNow,
                    UserAgent = Request.Headers["User-Agent"].ToString(),
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
                };

                var createdInteraction = await _interactionRepository.CreateAsync(interaction);
                
                _logger.LogInformation("User {UserId} disliked event {EventId}", userId, eventId);
                
                return Ok(new DislikeResponseDto
                {
                    Disliked = true,
                    Message = "Event disliked",
                    Interaction = new InteractionResponseDto
                    {
                        Id = createdInteraction.Id,
                        UserId = createdInteraction.UserId,
                        EventId = createdInteraction.EventId,
                        InteractionType = createdInteraction.InteractionType,
                        Timestamp = createdInteraction.Timestamp,
                        Metadata = createdInteraction.Metadata
                    },
                    Likes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Like),
                    Dislikes = await _interactionRepository.GetInteractionCountAsync(eventId, InteractionType.Dislike)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disliking event {EventId}", eventId);
                return StatusCode(500, new { 
                    message = "An error occurred while disliking the event",
                    error = ex.Message 
                });
            }
        }

       
        // GET /api/events/{eventId}/interactions/stats
        [HttpGet("events/{eventId}/interactions/stats")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EventStatsDto>> GetEventStats(Guid eventId)
        {
            try
            {
                var stats = await _interactionRepository.GetEventStatsAsync(eventId);
                
                // Get user interaction if authenticated
                InteractionType? userInteraction = null;
                if (User.Identity?.IsAuthenticated == true)
                {
                    var userId = GetCurrentUserId();
                    if (await _interactionRepository.HasInteractionAsync(userId, eventId, InteractionType.Like))
                    {
                        userInteraction = InteractionType.Like;
                    }
                    else if (await _interactionRepository.HasInteractionAsync(userId, eventId, InteractionType.Dislike))
                    {
                        userInteraction = InteractionType.Dislike;
                    }
                }
                
                var response = new EventStatsDto
                {
                    EventId = eventId,
                    Likes = stats.TryGetValue(InteractionType.Like, out int likes) ? likes : 0,
                    Dislikes = stats.TryGetValue(InteractionType.Dislike, out int dislikes) ? dislikes : 0,
                    Saves = stats.TryGetValue(InteractionType.Saved, out int saves) ? saves : 0,
                    Attended = stats.TryGetValue(InteractionType.Attended, out int attended) ? attended : 0,
                    UserInteraction = userInteraction
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting stats for event {EventId}", eventId);
                return StatusCode(500, new { 
                    message = "An error occurred while retrieving event stats",
                    error = ex.Message 
                });
            }
        }

        // GET /api/users/{userId}/interactions
        [HttpGet("users/{userId}/interactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InteractionResponseDto>>> GetUserInteractions(
            Guid userId, [FromQuery] InteractionType? type = null)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                
                // Users can only view their own interactions (or admin can view any)
                if (currentUserId != userId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }
                
                var interactions = await _interactionRepository.GetUserInteractionsAsync(userId, type);
                
                var response = interactions.Select(interaction => new InteractionResponseDto
                {
                    Id = interaction.Id,
                    UserId = interaction.UserId,
                    EventId = interaction.EventId,
                    InteractionType = interaction.InteractionType,
                    Timestamp = interaction.Timestamp,
                    Metadata = interaction.Metadata
                }).ToList();
                
                _logger.LogInformation("Retrieved {Count} interactions for user {UserId}", 
                    response.Count, userId);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting interactions for user {UserId}", userId);
                return StatusCode(500, new { 
                    message = "An error occurred while retrieving interactions",
                    error = ex.Message 
                });
            }
        }

        // GET /api/events/{eventId}/interactions
        [HttpGet("events/{eventId}/interactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InteractionResponseDto>>> GetEventInteractions(
            Guid eventId, [FromQuery] InteractionType? type = null)
        {
            try
            {
                var interactions = await _interactionRepository.GetEventInteractionsAsync(eventId, type);
                
                var response = interactions.Select(interaction => new InteractionResponseDto
                {
                    Id = interaction.Id,
                    UserId = interaction.UserId,
                    EventId = interaction.EventId,
                    InteractionType = interaction.InteractionType,
                    Timestamp = interaction.Timestamp,
                    Metadata = interaction.Metadata
                }).ToList();
                
                _logger.LogInformation("Retrieved {Count} interactions for event {EventId}", 
                    response.Count, eventId);
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting interactions for event {EventId}", eventId);
                return StatusCode(500, new { 
                    message = "An error occurred while retrieving event interactions",
                    error = ex.Message 
                });
            }
        }
    }
}