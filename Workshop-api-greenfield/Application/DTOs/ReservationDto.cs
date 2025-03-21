using System;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Data transfer object for reservation information.
    /// </summary>
    public class ReservationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the room being reserved.
        /// </summary>
        public Guid RoomId { get; set; }

        /// <summary>
        /// Gets or sets the name of the room being reserved.
        /// </summary>
        public string? RoomName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the user making the reservation.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the name of the user making the reservation.
        /// </summary>
        public string? UserName { get; set; }

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
        /// Gets or sets the status of the reservation.
        /// </summary>
        public ReservationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the reservation was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the reservation was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
} 