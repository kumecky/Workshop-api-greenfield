using System;
using System.Collections.Generic;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Data transfer object for calendar query parameters.
    /// </summary>
    public class CalendarQueryDto
    {
        /// <summary>
        /// Gets or sets the start date for the calendar view.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date for the calendar view.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the optional list of room IDs to filter by.
        /// If null or empty, all rooms will be included.
        /// </summary>
        public List<Guid>? RoomIds { get; set; }

        /// <summary>
        /// Gets or sets the optional user ID to filter by.
        /// If null, reservations for all users will be included.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the view type (day, week, month).
        /// </summary>
        public CalendarViewType ViewType { get; set; } = CalendarViewType.Week;
    }

    /// <summary>
    /// Represents the type of calendar view.
    /// </summary>
    public enum CalendarViewType
    {
        /// <summary>
        /// Day view showing reservations for a single day.
        /// </summary>
        Day,

        /// <summary>
        /// Week view showing reservations for 7 days.
        /// </summary>
        Week,

        /// <summary>
        /// Month view showing reservations for an entire month.
        /// </summary>
        Month
    }
} 