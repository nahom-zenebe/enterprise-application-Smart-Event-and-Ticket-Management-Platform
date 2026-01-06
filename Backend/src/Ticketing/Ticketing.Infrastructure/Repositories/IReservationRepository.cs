using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(Guid id);
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Reservation>> GetByEventIdAsync(Guid eventId);
        Task AddAsync(Reservation reservation);
        void Remove(Reservation reservation);
        Task SaveChangesAsync();
    }
}