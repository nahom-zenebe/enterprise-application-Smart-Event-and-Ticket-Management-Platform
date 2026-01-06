using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;

namespace Ticketing.API.Controllers
{
    [ApiController]
    [Route("reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public class PaymentRequest
        {
            public Guid PaymentId { get; set; }
        }

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReservationDto dto)
        {
            var reservation = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var reservation = await _service.GetByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            return Ok(await _service.GetByUserIdAsync(userId));
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetByEventId(Guid eventId)
        {
            return Ok(await _service.GetByEventIdAsync(eventId));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ReservationDto dto)
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
            try
            {
                await _service.ConfirmAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                await _service.CancelAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/tickets")]
        public async Task<IActionResult> AddTicket(Guid id, [FromBody] TicketDto ticketDto)
        {
            try
            {
                await _service.AddTicketAsync(id, ticketDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/payment")]
        public async Task<IActionResult> ProcessPayment(Guid id, [FromBody] PaymentRequest request)
        {
            try
            {
                // Process payment and confirm reservation
                await _service.ProcessPaymentAsync(id, request.PaymentId);
                return Ok(new { message = "Payment processed and reservation confirmed" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/total")]
        public async Task<IActionResult> GetTotal(Guid id)
        {
            try
            {
                var total = await _service.GetReservationTotalAsync(id);
                return Ok(new { total });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}