using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Persistence;

namespace Ticketing.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketingDbContext _context;

        public TicketRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
        }

        public async Task<Ticket> GetByIdAsync(Guid id)
        {
            return await _context.Tickets.FirstOrDefaultAsync(t => t.TicketID == id);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public void Remove(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
