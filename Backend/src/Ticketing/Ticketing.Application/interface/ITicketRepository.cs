using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketing.Application.DTOs;
using Ticketing.Domain.Entities;

namespace Ticketing.Application.Interfaces
{
    public interface ITicketService
    {
        Task<TicketDto> CreateAsync(TicketDto dto);
        Task<TicketDto> GetByIdAsync(Guid id);
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task UpdateAsync(Guid id, TicketDto dto);
        Task DeleteAsync(Guid id);

        Task ConfirmAsync(Guid id);
        Task CancelAsync(Guid id);
        Task CheckInAsync(Guid id);
    }
}
