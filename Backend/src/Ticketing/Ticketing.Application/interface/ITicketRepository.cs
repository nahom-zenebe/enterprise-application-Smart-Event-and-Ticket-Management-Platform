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
        
        Task<string> GetQRCodeAsync(Guid id);
        Task<ValidationResult> ValidateTicketAsync(ValidateTicketRequest request);
    }
    
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public TicketDto Ticket { get; set; }
    }
    
    public class ValidateTicketRequest
    {
        public Guid? TicketId { get; set; }
        public string QRCode { get; set; }
    }
}
