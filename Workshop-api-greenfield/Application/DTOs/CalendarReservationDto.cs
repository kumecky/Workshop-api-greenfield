using System;
using System.Text.Json.Serialization;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Data transfer object for reservation information in calendar view.
    /// </summary>
    public class CalendarReservationDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the reservation.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the title for the calendar event (typically the purpose of the reservation).
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start time of the reservation.
        /// </summary>
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the reservation.
        /// </summary>
        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the room identifier.
        /// </summary>
        [JsonPropertyName("roomId")]
        public Guid RoomId { get; set; }

        /// <summary>
        /// Gets or sets the room name.
        /// </summary>
        [JsonPropertyName("roomName")]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [JsonPropertyName("userName")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status of the reservation.
        /// </summary>
        [JsonPropertyName("status")]
        public ReservationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user is the creator of this reservation.
        /// </summary>
        [JsonPropertyName("isCurrentUserReservation")]
        public bool IsCurrentUserReservation { get; set; }

        /// <summary>
        /// Gets or sets additional information about the reservation.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the original purpose of the reservation (mapped to title).
        /// </summary>
        [JsonPropertyName("purpose")]
        public string? Purpose { get; set; }
    }
} 