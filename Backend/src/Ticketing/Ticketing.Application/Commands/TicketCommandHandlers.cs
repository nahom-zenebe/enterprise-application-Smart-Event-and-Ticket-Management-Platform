using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using Ticketing.Application.DTOs;
using Ticketing.Application.Interfaces;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure.Repositories;

namespace Ticketing.Application.Commands
{
    public class TicketCommandHandlers : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketCommandHandlers(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<TicketDto> CreateAsync(TicketDto dto)
        {
            var ticket = new Ticket(
                Guid.NewGuid(),
                dto.Type,
                dto.Price,
                dto.QRCode,
                dto.DiscountCode,
                dto.ReservationID,
                TicketStatus.Reserved,
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            await _repository.AddAsync(ticket);
            await _repository.SaveChangesAsync();

            dto.TicketID = ticket.TicketID;
            dto.Status = ticket.Status;
            return dto;
        }

        public async Task<TicketDto> GetByIdAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null) return null;

            return MapToDto(ticket);
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _repository.GetAllAsync();
            return tickets.Select(MapToDto);
        }

        public async Task UpdateAsync(Guid id, TicketDto dto)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null)
                throw new Exception("Ticket not found");

            ticket.Update(dto.Type, dto.Price, dto.DiscountCode, dto.QRCode);

            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null)
                throw new Exception("Ticket not found");

            _repository.Remove(ticket);
            await _repository.SaveChangesAsync();
        }

        public async Task ConfirmAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            ticket.Confirm();
            await _repository.SaveChangesAsync();
        }

        public async Task CancelAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            ticket.Cancel();
            await _repository.SaveChangesAsync();
        }

        public async Task CheckInAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            ticket.CheckIn();
            await _repository.SaveChangesAsync();
        }

        public async Task<string> GetQRCodeAsync(Guid id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null)
                throw new Exception("Ticket not found");

            // If QR code already exists, return it
            if (!string.IsNullOrWhiteSpace(ticket.QRCode))
            {
                return ticket.QRCode;
            }

            // Generate new QR code
            var qrCodeData = ticket.TicketID.ToString();
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrData = qrGenerator.CreateQrCode(qrCodeData, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qrCode = new QRCode(qrData))
                {
                    using (Bitmap qrBitmap = qrCode.GetGraphic(20))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            qrBitmap.Save(ms, ImageFormat.Png);
                            byte[] imageBytes = ms.ToArray();
                            string base64String = Convert.ToBase64String(imageBytes);
                            string qrCodeBase64 = $"data:image/png;base64,{base64String}";
                            
                            // Save QR code to ticket
                            ticket.Update(qrCode: qrCodeBase64);
                            await _repository.SaveChangesAsync();
                            
                            return qrCodeBase64;
                        }
                    }
                }
            }
        }

        public async Task<ValidationResult> ValidateTicketAsync(ValidateTicketRequest request)
        {
            Ticket ticket = null;

            // Find ticket by ID or QR code
            if (request.TicketId.HasValue)
            {
                ticket = await _repository.GetByIdAsync(request.TicketId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(request.QRCode))
            {
                // Try to extract ticket ID from QR code string
                // QR code contains the ticket ID as text, so try parsing it as GUID first
                if (Guid.TryParse(request.QRCode, out Guid ticketIdFromQr))
                {
                    ticket = await _repository.GetByIdAsync(ticketIdFromQr);
                }
                else
                {
                    // If not a direct GUID, search by matching QRCode field or ticket ID string
                    var allTickets = await _repository.GetAllAsync();
                    ticket = allTickets.FirstOrDefault(t => 
                        t.QRCode == request.QRCode || 
                        t.TicketID.ToString() == request.QRCode ||
                        (t.QRCode != null && t.QRCode.Contains(request.QRCode)));
                }
            }

            if (ticket == null)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Ticket not found",
                    Ticket = null
                };
            }

            // Validate ticket status
            if (ticket.Status == TicketStatus.Cancelled)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Ticket has been cancelled",
                    Ticket = MapToDto(ticket)
                };
            }

            if (ticket.Status == TicketStatus.CheckedIn)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Ticket has already been checked in",
                    Ticket = MapToDto(ticket)
                };
            }

            if (ticket.Status != TicketStatus.Confirmed)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = $"Ticket is not confirmed. Current status: {ticket.Status}",
                    Ticket = MapToDto(ticket)
                };
            }

            // Ticket is valid for entry - automatically check in
            try
            {
                ticket.CheckIn();
                await _repository.SaveChangesAsync();
                
                return new ValidationResult
                {
                    IsValid = true,
                    Message = "Ticket validated and checked in successfully",
                    Ticket = MapToDto(ticket)
                };
            }
            catch (InvalidOperationException ex)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = ex.Message,
                    Ticket = MapToDto(ticket)
                };
            }
        }

        private static TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                TicketID = ticket.TicketID,
                Type = ticket.Type,
                Price = ticket.Price,
                DiscountCode = ticket.DiscountCode,
                Status = ticket.Status,
                ReservationID = ticket.ReservationID,
                QRCode = ticket.QRCode,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt
            };
        }
    }
}
