using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventPlanning.Domain.Entities;

namespace EventPlanning.Application.Interfaces
{
    public interface IVenueRepository
    {
        Task<Venue> CreateAsync(Venue venue);
        Task<List<Venue>> GetAllAsync();
        Task<Venue?> GetByIdAsync(Guid id);
        Task<Venue> UpdateAsync(Venue venue);
        Task<bool> DeleteAsync(Guid id);
    }
}