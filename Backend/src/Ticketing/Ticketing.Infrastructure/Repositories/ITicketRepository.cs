using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.Repositories
{
    public interface ITicketRepository
    {
        Task AddAsync(Ticket ticket);
        Task<Ticket> GetByIdAsync(Guid id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        void Remove(Ticket ticket);
        Task SaveChangesAsync();
    }
}
