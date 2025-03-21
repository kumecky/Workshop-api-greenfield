using System;
using System.Collections.Generic;

namespace Workshop_api_greenfield.Domain.Entities
{
    /// <summary>
    /// Represents an office room that can be reserved.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        public required string Name { get; set; }

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

        /// <summary>
        /// Gets or sets the list of reservations for this room.
        /// </summary>
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
} 