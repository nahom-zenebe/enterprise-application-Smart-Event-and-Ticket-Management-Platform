using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("access")]
    public class AccessController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public AccessController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <summary>
        /// Validate QR code at entry
        /// </summary>
        /// <param name="request">Request containing QR code or ticket ID</param>
        /// <returns>Validation result</returns>
        [HttpPost("validate")]
        public async Task<IActionResult> Validate([FromBody] ValidateTicketRequest request)
        {
            if (request == null || (!request.TicketId.HasValue && string.IsNullOrWhiteSpace(request.QRCode)))
            {
                return BadRequest(new { message = "Either TicketId or QRCode must be provided" });
            }

            try
            {
                var result = await _ticketService.ValidateTicketAsync(request);
                
                if (result.IsValid)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get access pass status for a ticket
        /// </summary>
        /// <param name="id">Ticket ID</param>
        /// <returns>Access pass status information</returns>
        [HttpGet("ticket/{id}")]
        public async Task<IActionResult> GetAccessPassStatus(Guid id)
        {
            try
            {
                var ticket = await _ticketService.GetByIdAsync(id);
                
                if (ticket == null)
                {
                    return NotFound(new { message = "Ticket not found" });
                }

                var accessPassStatus = new AccessPassStatusDto
                {
                    TicketId = ticket.TicketID,
                    Status = ticket.Status.ToString(),
                    HasAccess = ticket.Status == TicketStatus.Confirmed || 
                               ticket.Status == TicketStatus.CheckedIn,
                    TicketType = ticket.Type.ToString(),
                    IsCheckedIn = ticket.Status == TicketStatus.CheckedIn,
                    IsCancelled = ticket.Status == TicketStatus.Cancelled,
                    CreatedAt = ticket.CreatedAt,
                    UpdatedAt = ticket.UpdatedAt
                };

                return Ok(accessPassStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    /// <summary>
    /// DTO for access pass status response
    /// </summary>
    public class AccessPassStatusDto
    {
        public Guid TicketId { get; set; }
        public string Status { get; set; }
        public bool HasAccess { get; set; }
        public string TicketType { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

