using EventPlanning.Domain.Entities;

namespace EventPlanning.Application.Interfaces
{
    public interface IPerformerRepository
    {
        Task<Performer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Performer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Performer> AddAsync(Performer performer, CancellationToken cancellationToken = default);
        Task UpdateAsync(Performer performer, CancellationToken cancellationToken = default);
        Task DeleteAsync(Performer performer, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}