using EventPlanning.Application.Interfaces;
using EventPlanning.Domain.Entities;
using EventPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventPlanning.Infrastructure.Repositories
{
    public class PerformerRepository : IPerformerRepository
    {
        private readonly EventPlanningDbContext _context;

        public PerformerRepository(EventPlanningDbContext context)
        {
            _context = context;
        }

        public async Task<Performer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Performers
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Performer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Performers
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Performer> AddAsync(Performer performer, CancellationToken cancellationToken = default)
        {
            _context.Performers.Add(performer);
            await _context.SaveChangesAsync(cancellationToken);
            return performer;
        }

        public async Task UpdateAsync(Performer performer, CancellationToken cancellationToken = default)
        {
            performer.UpdatedAt = DateTime.UtcNow;
            _context.Performers.Update(performer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Performer performer, CancellationToken cancellationToken = default)
        {
            _context.Performers.Remove(performer);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Performers
                .AnyAsync(p => p.Id == id, cancellationToken);
        }
    }
}