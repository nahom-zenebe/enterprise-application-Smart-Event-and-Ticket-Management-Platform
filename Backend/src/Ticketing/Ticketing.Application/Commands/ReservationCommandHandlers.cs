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
    public class ReservationCommandHandlers : IReservationService
    {
        private readonly IReservationRepository _repository;
        private readonly ITicketRepository _ticketRepository;

        public ReservationCommandHandlers(IReservationRepository repository, ITicketRepository ticketRepository)
        {
            _repository = repository;
            _ticketRepository = ticketRepository;
        }

        public async Task<ReservationDto> CreateAsync(ReservationDto dto)
        {
            var reservation = new Reservation(dto.UserId, dto.EventId, dto.ExpiresAt);

            await _repository.AddAsync(reservation);
            await _repository.SaveChangesAsync();

            return MapToDto(reservation);
        }

        public async Task<ReservationDto> GetByIdAsync(Guid id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            return reservation == null ? null : MapToDto(reservation);
        }

        public async Task<IEnumerable<ReservationDto>> GetAllAsync()
        {
            var reservations = await _repository.GetAllAsync();
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationDto>> GetByUserIdAsync(Guid userId)
        {
            var reservations = await _repository.GetByUserIdAsync(userId);
            return reservations.Select(MapToDto);
        }

        public async Task<IEnumerable<ReservationDto>> GetByEventIdAsync(Guid eventId)
        {
            var reservations = await _repository.GetByEventIdAsync(eventId);
            return reservations.Select(MapToDto);
        }

        public async Task UpdateAsync(Guid id, ReservationDto dto)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            _repository.Remove(reservation);
            await _repository.SaveChangesAsync();
        }

        public async Task ConfirmAsync(Guid id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            reservation.Confirm();
            await _repository.SaveChangesAsync();
        }

        public async Task CancelAsync(Guid id)
        {
            var reservation = await _repository.GetByIdAsync(id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            reservation.Cancel();
            await _repository.SaveChangesAsync();
        }

        public async Task AddTicketAsync(Guid reservationId, TicketDto ticketDto)
        {
            var reservation = await _repository.GetByIdAsync(reservationId);
            if (reservation == null)
                throw new Exception("Reservation not found");

            var ticket = new Ticket(
                Guid.NewGuid(),
                ticketDto.Type,
                ticketDto.Price,
                ticketDto.QRCode,
                ticketDto.DiscountCode,
                reservationId,
                TicketStatus.Reserved,
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            reservation.AddTicket(ticket);
            await _ticketRepository.AddAsync(ticket);
            await _repository.SaveChangesAsync();
        }

        private static ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                UserId = reservation.UserId,
                EventId = reservation.EventId,
                Status = reservation.Status,
                ReservedAt = reservation.ReservedAt,
                ExpiresAt = reservation.ExpiresAt,
                Tickets = reservation.Tickets.Select(t => new TicketDto
                {
                    TicketID = t.TicketID,
                    Type = t.Type,
                    Price = t.Price,
                    DiscountCode = t.DiscountCode,
                    Status = t.Status,
                    ReservationID = t.ReservationID,
                    QRCode = t.QRCode,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                }).ToList()
            };
        }
    }
}