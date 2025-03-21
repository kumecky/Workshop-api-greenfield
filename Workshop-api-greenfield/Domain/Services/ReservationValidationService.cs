using System;
using System.Collections.Generic;
using System.Linq;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Domain.Services
{
    /// <summary>
    /// Service for validating reservation business rules.
    /// </summary>
    public class ReservationValidationService
    {
        /// <summary>
        /// Validates if a reservation can be created.
        /// </summary>
        /// <param name="reservation">The reservation to validate.</param>
        /// <param name="existingReservations">The list of existing reservations for the room.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns>True if the reservation is valid, false otherwise.</returns>
        public bool ValidateReservation(Reservation reservation, IEnumerable<Reservation> existingReservations, out string errorMessage)
        {
            // Check if end time is after start time
            if (reservation.EndTime <= reservation.StartTime)
            {
                errorMessage = "End time must be after start time.";
                return false;
            }

            // Check if reservation is not in the past
            if (reservation.StartTime < DateTime.UtcNow)
            {
                errorMessage = "Cannot create reservations in the past.";
                return false;
            }

            // Check for overlapping reservations (exclude cancelled reservations)
            var activeReservations = existingReservations.Where(r => r.Status != ReservationStatus.Cancelled && r.Id != reservation.Id);
            
            foreach (var existingReservation in activeReservations)
            {
                if (reservation.OverlapsWith(existingReservation))
                {
                    errorMessage = $"Reservation overlaps with an existing reservation from {existingReservation.StartTime} to {existingReservation.EndTime}.";
                    return false;
                }
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates if a user can cancel a reservation.
        /// </summary>
        /// <param name="reservation">The reservation to cancel.</param>
        /// <param name="userId">The ID of the user attempting to cancel.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns>True if the cancellation is valid, false otherwise.</returns>
        public bool ValidateCancellation(Reservation reservation, Guid userId, out string errorMessage)
        {
            // Check if the reservation is already cancelled
            if (reservation.Status == ReservationStatus.Cancelled)
            {
                errorMessage = "Reservation is already cancelled.";
                return false;
            }

            // Check if the user is the one who created the reservation
            if (reservation.UserId != userId)
            {
                errorMessage = "Only the user who created the reservation can cancel it.";
                return false;
            }

            // Check if the reservation is in the past
            if (reservation.StartTime < DateTime.UtcNow)
            {
                errorMessage = "Cannot cancel reservations that have already started or ended.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
} 