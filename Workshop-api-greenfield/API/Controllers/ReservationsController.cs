using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workshop_api_greenfield.Application.DTOs;
using Workshop_api_greenfield.Application.Services;

namespace Workshop_api_greenfield.API.Controllers
{
    /// <summary>
    /// API controller for managing reservations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationService _reservationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationsController"/> class.
        /// </summary>
        /// <param name="reservationService">The reservation service.</param>
        public ReservationsController(ReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        /// <summary>
        /// Gets all reservations.
        /// </summary>
        /// <returns>A collection of all reservations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), 200)]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        /// <summary>
        /// Gets a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to get.</param>
        /// <returns>The reservation with the specified ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ReservationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReservationById(Guid id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            
            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="createDto">The data for the new reservation.</param>
        /// <returns>The created reservation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ReservationDto), 201)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto createDto)
        {
            var (reservation, errorMessage) = await _reservationService.CreateReservationAsync(createDto);
            
            if (reservation == null)
            {
                ModelState.AddModelError("Error", errorMessage);
                return ValidationProblem(ModelState);
            }

            return CreatedAtAction(nameof(GetReservationById), new { id = reservation.Id }, reservation);
        }

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="updateDto">The updated reservation data.</param>
        /// <returns>The updated reservation.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ReservationDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateReservation(Guid id, [FromBody] CreateReservationDto updateDto)
        {
            var (reservation, errorMessage) = await _reservationService.UpdateReservationAsync(id, updateDto);
            
            if (reservation == null)
            {
                if (errorMessage == "Reservation not found.")
                {
                    return NotFound();
                }

                ModelState.AddModelError("Error", errorMessage);
                return ValidationProblem(ModelState);
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Cancels a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to cancel.</param>
        /// <param name="userId">The ID of the user cancelling the reservation.</param>
        /// <returns>The cancelled reservation.</returns>
        [HttpPatch("{id}/cancel")]
        [ProducesResponseType(typeof(ReservationDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CancelReservation(Guid id, [FromQuery] Guid userId)
        {
            var (reservation, errorMessage) = await _reservationService.CancelReservationAsync(id, userId);
            
            if (reservation == null)
            {
                if (errorMessage == "Reservation not found.")
                {
                    return NotFound();
                }

                ModelState.AddModelError("Error", errorMessage);
                return ValidationProblem(ModelState);
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Deletes a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteReservation(Guid id)
        {
            var result = await _reservationService.DeleteReservationAsync(id);
            
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
} 