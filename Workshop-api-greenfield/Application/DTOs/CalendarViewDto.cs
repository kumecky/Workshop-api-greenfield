using System;
using System.Collections.Generic;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Data transfer object for calendar view.
    /// </summary>
    public class CalendarViewDto
    {
        /// <summary>
        /// Gets or sets the start date of the calendar view.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the calendar view.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the list of reservations in the calendar view.
        /// </summary>
        public List<CalendarReservationDto> Reservations { get; set; } = new List<CalendarReservationDto>();

        /// <summary>
        /// Gets or sets the list of room identifiers included in the view.
        /// </summary>
        public List<Guid> RoomIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Gets or sets a dictionary mapping room IDs to their names for easy reference.
        /// </summary>
        public Dictionary<Guid, string> RoomNames { get; set; } = new Dictionary<Guid, string>();
    }
} 