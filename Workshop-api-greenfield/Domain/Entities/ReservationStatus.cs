namespace Workshop_api_greenfield.Domain.Entities
{
    /// <summary>
    /// Represents the different statuses a reservation can have.
    /// </summary>
    public enum ReservationStatus
    {
        /// <summary>
        /// The reservation has been confirmed.
        /// </summary>
        Confirmed,

        /// <summary>
        /// The reservation is pending confirmation.
        /// </summary>
        Pending,

        /// <summary>
        /// The reservation has been cancelled.
        /// </summary>
        Cancelled
    }
} 