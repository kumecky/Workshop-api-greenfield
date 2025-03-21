using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.Services
{
    /// <summary>
    /// Interface for the reservation repository.
    /// </summary>
    public interface IReservationRepository
    {
        /// <summary>
        /// Gets all reservations.
        /// </summary>
        /// <returns>A collection of all reservations.</returns>
        Task<IEnumerable<Reservation>> GetAllAsync();

        /// <summary>
        /// Gets a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to get.</param>
        /// <returns>The reservation with the specified ID, or null if not found.</returns>
        Task<Reservation?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets reservations for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room.</param>
        /// <returns>A collection of reservations for the specified room.</returns>
        Task<IEnumerable<Reservation>> GetByRoomIdAsync(Guid roomId);

        /// <summary>
        /// Gets reservations for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A collection of reservations for the specified user.</returns>
        Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a new reservation.
        /// </summary>
        /// <param name="reservation">The reservation to add.</param>
        /// <returns>The added reservation.</returns>
        Task<Reservation> AddAsync(Reservation reservation);

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="reservation">The reservation to update.</param>
        /// <returns>The updated reservation.</returns>
        Task<Reservation> UpdateAsync(Reservation reservation);

        /// <summary>
        /// Deletes a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>True if the reservation was deleted, false otherwise.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
} 