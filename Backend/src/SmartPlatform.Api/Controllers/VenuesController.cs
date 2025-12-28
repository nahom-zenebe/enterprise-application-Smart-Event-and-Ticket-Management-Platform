using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPlanning.Application.Interfaces;
using EventPlanning.Application.DTOs;
using EventPlanning.Domain.Entities;

namespace SmartPlatform.Api.Controllers
{
    [ApiController]
    [Route("api/venues")]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueRepository _venueRepository;

        public VenuesController(IVenueRepository venueRepository)
        {
            _venueRepository = venueRepository;
        }

        // GET /api/venues
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<VenueResponseDto>>> GetVenues()
        {
            var venues = await _venueRepository.GetAllAsync();
            
            var response = venues.Select(venue => new VenueResponseDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Location = venue.Location,
                Capacity = venue.Capacity,
                SeatingLayoutId = venue.SeatingLayoutId,
                ContactInfo = venue.ContactInfo,
                CreatedAt = venue.CreatedAt,
                UpdatedAt = venue.UpdatedAt
            }).ToList();

            return Ok(response);
        }

        // POST /api/venues
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VenueResponseDto>> CreateVenue(
            [FromBody] CreateVenueDto createVenueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var venue = new Venue(
                createVenueDto.Name,
                createVenueDto.Location,
                createVenueDto.Capacity,
                createVenueDto.SeatingLayoutId,
                createVenueDto.ContactInfo);

            var createdVenue = await _venueRepository.CreateAsync(venue);

            var response = new VenueResponseDto
            {
                Id = createdVenue.Id,
                Name = createdVenue.Name,
                Location = createdVenue.Location,
                Capacity = createdVenue.Capacity,
                SeatingLayoutId = createdVenue.SeatingLayoutId,
                ContactInfo = createdVenue.ContactInfo,
                CreatedAt = createdVenue.CreatedAt,
                UpdatedAt = createdVenue.UpdatedAt
            };

            return CreatedAtAction(nameof(GetVenues), response);
        }

        // PUT /api/venues/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VenueResponseDto>> UpdateVenue(
            Guid id, 
            [FromBody] UpdateVenueDto updateVenueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingVenue = await _venueRepository.GetByIdAsync(id);
            if (existingVenue == null)
                return NotFound($"Venue with ID {id} not found");

            existingVenue.Update(
                updateVenueDto.Name,
                updateVenueDto.Location,
                updateVenueDto.Capacity,
                updateVenueDto.SeatingLayoutId,
                updateVenueDto.ContactInfo);

            var updatedVenue = await _venueRepository.UpdateAsync(existingVenue);

            var response = new VenueResponseDto
            {
                Id = updatedVenue.Id,
                Name = updatedVenue.Name,
                Location = updatedVenue.Location,
                Capacity = updatedVenue.Capacity,
                SeatingLayoutId = updatedVenue.SeatingLayoutId,
                ContactInfo = updatedVenue.ContactInfo,
                CreatedAt = updatedVenue.CreatedAt,
                UpdatedAt = updatedVenue.UpdatedAt
            };

            return Ok(response);
        }
    }
}