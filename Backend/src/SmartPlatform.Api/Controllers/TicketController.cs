using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;

namespace Ticketing.API.Controllers
{
    [ApiController]
    [Route("tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _service;

        public TicketController(ITicketService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(TicketDto dto)
        {
            var ticket = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = ticket.TicketID }, ticket);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ticket = await _service.GetByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TicketDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> Confirm(Guid id)
        {
            await _service.ConfirmAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _service.CancelAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/checkin")]
        public async Task<IActionResult> CheckIn(Guid id)
        {
            await _service.CheckInAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/qrcode")]
        public async Task<IActionResult> GetQRCode(Guid id)
        {
            try
            {
                var qrCode = await _service.GetQRCodeAsync(id);
                return Ok(new { qrCode });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Ticket not found")
                    return NotFound(new { message = ex.Message });
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateTicket([FromBody] ValidateTicketRequest request)
        {
            if (request == null || (!request.TicketId.HasValue && string.IsNullOrWhiteSpace(request.QRCode)))
            {
                return BadRequest(new { message = "Either TicketId or QRCode must be provided" });
            }

            try
            {
                var result = await _service.ValidateTicketAsync(request);
                
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
    }
}
