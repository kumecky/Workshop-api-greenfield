using System;
using System.Collections.Generic;

namespace Workshop_api_greenfield.Domain.Entities
{
    /// <summary>
    /// Represents a user who can make room reservations.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the department the user belongs to.
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// Gets or sets the list of reservations made by this user.
        /// </summary>
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
} 