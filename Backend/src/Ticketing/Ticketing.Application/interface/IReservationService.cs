using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;

namespace Ticketing.Application.Interfaces
{
    public interface IReservationService
    {
        Task<ReservationDto> CreateAsync(ReservationDto dto);
        Task<ReservationDto> GetByIdAsync(Guid id);
        Task<IEnumerable<ReservationDto>> GetAllAsync();
        Task<IEnumerable<ReservationDto>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<ReservationDto>> GetByEventIdAsync(Guid eventId);
        Task UpdateAsync(Guid id, ReservationDto dto);
        Task DeleteAsync(Guid id);
        Task ConfirmAsync(Guid id);
        Task CancelAsync(Guid id);
        Task AddTicketAsync(Guid reservationId, TicketDto ticketDto);
    }
}