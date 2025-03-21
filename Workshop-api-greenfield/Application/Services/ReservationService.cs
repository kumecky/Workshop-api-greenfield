using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workshop_api_greenfield.Application.DTOs;
using Workshop_api_greenfield.Domain.Entities;
using Workshop_api_greenfield.Domain.Services;

namespace Workshop_api_greenfield.Application.Services
{
    /// <summary>
    /// Service for handling reservation operations.
    /// </summary>
    public class ReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly ReservationValidationService _validationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationService"/> class.
        /// </summary>
        /// <param name="reservationRepository">The reservation repository.</param>
        /// <param name="roomRepository">The room repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="validationService">The reservation validation service.</param>
        public ReservationService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository,
            ReservationValidationService validationService)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _validationService = validationService;
        }

        /// <summary>
        /// Gets all reservations.
        /// </summary>
        /// <returns>A collection of all reservations.</returns>
        public async Task<IEnumerable<ReservationDto>> GetAllReservationsAsync()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            return reservations.Select(MapToDto);
        }

        /// <summary>
        /// Gets a reservation by its ID.
        /// </summary>
        /// <param name="id">The ID of the reservation to get.</param>
        /// <returns>The reservation with the specified ID, or null if not found.</returns>
        public async Task<ReservationDto?> GetReservationByIdAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);
            return reservation != null ? MapToDto(reservation) : null;
        }

        /// <summary>
        /// Creates a new reservation.
        /// </summary>
        /// <param name="createDto">The data for the new reservation.</param>
        /// <returns>The created reservation, or null if creation failed.</returns>
        public async Task<(ReservationDto? Reservation, string ErrorMessage)> CreateReservationAsync(CreateReservationDto createDto)
        {
            // Check if room exists
            var room = await _roomRepository.GetByIdAsync(createDto.RoomId);
            if (room == null)
            {
                return (null, "Room not found.");
            }

            // Check if user exists
            var user = await _userRepository.GetByIdAsync(createDto.UserId);
            if (user == null)
            {
                return (null, "User not found.");
            }

            // Create reservation entity
            var reservation = new Reservation
            {
                RoomId = createDto.RoomId,
                UserId = createDto.UserId,
                StartTime = createDto.StartTime,
                EndTime = createDto.EndTime,
                Purpose = createDto.Purpose,
                Status = ReservationStatus.Pending
            };

            // Validate reservation
            var existingReservations = await _reservationRepository.GetByRoomIdAsync(createDto.RoomId);
            if (!_validationService.ValidateReservation(reservation, existingReservations, out string errorMessage))
            {
                return (null, errorMessage);
            }

            // Save reservation
            var createdReservation = await _reservationRepository.AddAsync(reservation);
            return (MapToDto(createdReservation), string.Empty);
        }

        /// <summary>
        /// Updates an existing reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to update.</param>
        /// <param name="updateDto">The updated reservation data.</param>
        /// <returns>The updated reservation, or null if update failed.</returns>
        public async Task<(ReservationDto? Reservation, string ErrorMessage)> UpdateReservationAsync(Guid id, CreateReservationDto updateDto)
        {
            // Check if reservation exists
            var existingReservation = await _reservationRepository.GetByIdAsync(id);
            if (existingReservation == null)
            {
                return (null, "Reservation not found.");
            }

            // Check if room exists
            var room = await _roomRepository.GetByIdAsync(updateDto.RoomId);
            if (room == null)
            {
                return (null, "Room not found.");
            }

            // Check if user exists
            var user = await _userRepository.GetByIdAsync(updateDto.UserId);
            if (user == null)
            {
                return (null, "User not found.");
            }

            // Update reservation properties
            existingReservation.RoomId = updateDto.RoomId;
            existingReservation.UserId = updateDto.UserId;
            existingReservation.StartTime = updateDto.StartTime;
            existingReservation.EndTime = updateDto.EndTime;
            existingReservation.Purpose = updateDto.Purpose;
            existingReservation.UpdatedAt = DateTime.UtcNow;

            // Validate reservation
            var existingReservations = await _reservationRepository.GetByRoomIdAsync(updateDto.RoomId);
            if (!_validationService.ValidateReservation(existingReservation, existingReservations, out string errorMessage))
            {
                return (null, errorMessage);
            }

            // Save updated reservation
            var updatedReservation = await _reservationRepository.UpdateAsync(existingReservation);
            return (MapToDto(updatedReservation), string.Empty);
        }

        /// <summary>
        /// Cancels a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to cancel.</param>
        /// <param name="userId">The ID of the user cancelling the reservation.</param>
        /// <returns>The cancelled reservation, or null if cancellation failed.</returns>
        public async Task<(ReservationDto? Reservation, string ErrorMessage)> CancelReservationAsync(Guid id, Guid userId)
        {
            // Check if reservation exists
            var existingReservation = await _reservationRepository.GetByIdAsync(id);
            if (existingReservation == null)
            {
                return (null, "Reservation not found.");
            }

            // Validate cancellation
            if (!_validationService.ValidateCancellation(existingReservation, userId, out string errorMessage))
            {
                return (null, errorMessage);
            }

            // Update reservation status
            existingReservation.Status = ReservationStatus.Cancelled;
            existingReservation.UpdatedAt = DateTime.UtcNow;

            // Save updated reservation
            var updatedReservation = await _reservationRepository.UpdateAsync(existingReservation);
            return (MapToDto(updatedReservation), string.Empty);
        }

        /// <summary>
        /// Deletes a reservation.
        /// </summary>
        /// <param name="id">The ID of the reservation to delete.</param>
        /// <returns>True if the reservation was deleted, false otherwise.</returns>
        public async Task<bool> DeleteReservationAsync(Guid id)
        {
            return await _reservationRepository.DeleteAsync(id);
        }

        private ReservationDto MapToDto(Reservation reservation)
        {
            return new ReservationDto
            {
                Id = reservation.Id,
                RoomId = reservation.RoomId,
                RoomName = reservation.Room?.Name,
                UserId = reservation.UserId,
                UserName = reservation.User?.Name,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                Purpose = reservation.Purpose,
                Status = reservation.Status,
                CreatedAt = reservation.CreatedAt,
                UpdatedAt = reservation.UpdatedAt
            };
        }
    }
} 