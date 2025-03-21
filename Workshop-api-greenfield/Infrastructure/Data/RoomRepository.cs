using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workshop_api_greenfield.Application.Services;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Infrastructure.Data
{
    /// <summary>
    /// Implementation of the room repository interface.
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly ReservationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public RoomRepository(ReservationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Reservations)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _context.Rooms
                .Include(r => r.Reservations)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime startTime, DateTime endTime)
        {
            // Get all rooms
            var rooms = await _context.Rooms
                .Include(r => r.Reservations)
                .ToListAsync();

            // Filter rooms that don't have any active reservations in the specified time slot
            var availableRooms = rooms.Where(r => !r.Reservations
                .Any(res => 
                    res.Status != ReservationStatus.Cancelled && 
                    startTime < res.EndTime && 
                    endTime > res.StartTime)
            ).ToList();

            return availableRooms;
        }

        /// <inheritdoc/>
        public async Task<Room> AddAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        /// <inheritdoc/>
        public async Task<Room> UpdateAsync(Room room)
        {
            _context.Entry(room).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return room;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return false;
            }

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 