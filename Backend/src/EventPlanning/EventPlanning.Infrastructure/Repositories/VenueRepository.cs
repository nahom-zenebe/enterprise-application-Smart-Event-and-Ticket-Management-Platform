using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlanning.Application.Interfaces;
using EventPlanning.Domain.Entities;
using EventPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Infrastructure.Repositories
{
    public class VenueRepository : IVenueRepository
    {
        private readonly EventPlanningDbContext _context;

        public VenueRepository(EventPlanningDbContext context)
        {
            _context = context;
        }

        public async Task<Venue> CreateAsync(Venue venue)
        {
            // Ensure CreatedAt is set
            if (venue.CreatedAt == default)
                venue.CreatedAt = DateTime.UtcNow;
                
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return venue;
        }

        public async Task<List<Venue>> GetAllAsync()
        {
            return await _context.Venues
                .OrderBy(v => v.Name)
                .ToListAsync();
        }

        public async Task<Venue?> GetByIdAsync(Guid id)
        {
            return await _context.Venues.FindAsync(id);
        }

        public async Task<Venue> UpdateAsync(Venue venue)
        {
            venue.UpdatedAt = DateTime.UtcNow;
            _context.Venues.Update(venue);
            await _context.SaveChangesAsync();
            return venue;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
                return false;

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}