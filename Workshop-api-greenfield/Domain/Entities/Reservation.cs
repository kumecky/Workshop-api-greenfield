using System;

namespace Workshop_api_greenfield.Domain.Entities
{
    /// <summary>
    /// Represents a reservation for a room made by a user.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the ID of the room being reserved.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Gets or sets the room being reserved.
        /// </summary>
        public Room? Room { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user making the reservation.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user making the reservation.
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Gets or sets the start time of the reservation.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the reservation.
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the purpose of the reservation.
        /// </summary>
        public string? Purpose { get; set; }

        /// <summary>
        /// Gets or sets additional notes about the reservation.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the status of the reservation.
        /// </summary>
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        /// <summary>
        /// Gets or sets the date and time when the reservation was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the date and time when the reservation was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Checks if the reservation time slot overlaps with another reservation.
        /// </summary>
        /// <param name="other">The other reservation to check against.</param>
        /// <returns>True if the reservations overlap, false otherwise.</returns>
        public bool OverlapsWith(Reservation other)
        {
            return (StartTime < other.EndTime && EndTime > other.StartTime);
        }
    }
} 