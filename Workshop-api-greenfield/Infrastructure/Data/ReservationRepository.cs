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
    /// Implementation of the reservation repository interface.
    /// </summary>
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ReservationRepository(ReservationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Reservation?> GetByIdAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetByRoomIdAsync(Guid roomId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Where(r => r.RoomId == roomId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Room)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        /// <inheritdoc/>
        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            reservation.UpdatedAt = DateTime.UtcNow;
            _context.Entry(reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return reservation;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return false;
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 