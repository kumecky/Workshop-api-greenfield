using System;
using System.Collections.Generic;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Data transfer object for room information.
    /// </summary>
    public class RoomDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the capacity of the room (number of people).
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets or sets a description of the room.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the floor on which the room is located.
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// Gets or sets the list of features available in the room.
        /// </summary>
        public List<RoomFeature> Features { get; set; } = new List<RoomFeature>();
    }
} 