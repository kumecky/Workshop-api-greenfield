using System;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Data transfer object for creating a new reservation.
    /// </summary>
    public class CreateReservationDto
    {
        /// <summary>
        /// Gets or sets the ID of the room to reserve.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user making the reservation.
        /// </summary>
        public Guid UserId { get; set; }

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
    }
} 