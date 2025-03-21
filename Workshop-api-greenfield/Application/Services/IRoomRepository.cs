using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.Services
{
    /// <summary>
    /// Interface for the room repository.
    /// </summary>
    public interface IRoomRepository
    {
        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>A collection of all rooms.</returns>
        Task<IEnumerable<Room>> GetAllAsync();

        /// <summary>
        /// Gets a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to get.</param>
        /// <returns>The room with the specified ID, or null if not found.</returns>
        Task<Room?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets available rooms for a specific time slot.
        /// </summary>
        /// <param name="startTime">The start time of the time slot.</param>
        /// <param name="endTime">The end time of the time slot.</param>
        /// <returns>A collection of available rooms for the specified time slot.</returns>
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime);

        /// <summary>
        /// Adds a new room.
        /// </summary>
        /// <param name="room">The room to add.</param>
        /// <returns>The added room.</returns>
        Task<Room> AddAsync(Room room);

        /// <summary>
        /// Updates an existing room.
        /// </summary>
        /// <param name="room">The room to update.</param>
        /// <returns>The updated room.</returns>
        Task<Room> UpdateAsync(Room room);

        /// <summary>
        /// Deletes a room.
        /// </summary>
        /// <param name="id">The ID of the room to delete.</param>
        /// <returns>True if the room was deleted, false otherwise.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
} 