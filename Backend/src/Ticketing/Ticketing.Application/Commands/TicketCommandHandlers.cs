using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Repositories;

namespace Ticketing.Application.Commands
{
    public class TicketCommandHandlers : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketCommandHandlers(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<TicketDto> CreateAsync(TicketDto dto)
        {
            var ticket = new Ticket(
                Guid.NewGuid(),
                dto.Type,
                dto.Price,
                dto.QRCode,
                dto.DiscountCode,
                dto.ReservationID,
                TicketStatus.Reserved,
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            await _repository.AddAsync(ticket);
            await _repository.SaveChangesAsync();

            dto.TicketID = ticket.TicketID;
            dto.Status = ticket.Status;
            return dto;
        }

        public async Task<TicketDto> GetByIdAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null) return null;

            return MapToDto(ticket);
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _repository.GetAllAsync();
            return tickets.Select(MapToDto);
        }

        public async Task UpdateAsync(Guid id, TicketDto dto)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null)
                throw new Exception("Ticket not found");

            ticket.Update(dto.Type, dto.Price, dto.DiscountCode, dto.QRCode);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null)
                throw new Exception("Ticket not found");

            _repository.Remove(ticket);
            await _repository.SaveChangesAsync();
        }

        public async Task ConfirmAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            ticket.Confirm();
            await _repository.SaveChangesAsync();
        }

        public async Task CancelAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            ticket.Cancel();
            await _repository.SaveChangesAsync();
        }

        public async Task CheckInAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            ticket.CheckIn();
            await _repository.SaveChangesAsync();
        }

        private static TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                TicketID = ticket.TicketID,
                Type = ticket.Type,
                Price = ticket.Price,
                DiscountCode = ticket.DiscountCode,
                Status = ticket.Status,
                ReservationID = ticket.ReservationID,
                QRCode = ticket.QRCode,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }
    }
}
