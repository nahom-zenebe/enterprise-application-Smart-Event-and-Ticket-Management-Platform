using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Persistence;

namespace Ticketing.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly TicketingDbContext _context;

        public ReservationRepository(TicketingDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> GetByIdAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByEventIdAsync(Guid eventId)
        {
            return await _context.Reservations
                .Include(r => r.Tickets)
                .Where(r => r.EventId == eventId)
                .ToListAsync();
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
        }

        public void Remove(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}