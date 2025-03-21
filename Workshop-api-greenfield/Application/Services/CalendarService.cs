using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workshop_api_greenfield.Application.DTOs;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.Services
{
    /// <summary>
    /// Service for handling calendar-related operations.
    /// </summary>
    public class CalendarService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarService"/> class.
        /// </summary>
        /// <param name="reservationRepository">The reservation repository.</param>
        /// <param name="roomRepository">The room repository.</param>
        /// <param name="userRepository">The user repository.</param>
        public CalendarService(
            IReservationRepository reservationRepository,
            IRoomRepository roomRepository,
            IUserRepository userRepository)
        {
            _reservationRepository = reservationRepository;
            _roomRepository = roomRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Gets calendar data based on the provided query parameters.
        /// </summary>
        /// <param name="query">The calendar query parameters.</param>
        /// <param name="currentUserId">The ID of the current user, if available.</param>
        /// <returns>A calendar view DTO containing reservations and related information.</returns>
        public async Task<CalendarViewDto> GetCalendarDataAsync(CalendarQueryDto query, Guid? currentUserId = null)
        {
            // Validate and adjust dates based on view type
            (DateTime startDate, DateTime endDate) = AdjustDatesForViewType(query.StartDate, query.EndDate, query.ViewType);

            // Fetch reservations for the specified date range and filters
            var reservations = await _reservationRepository.GetForCalendarAsync(
                startDate,
                endDate,
                query.RoomIds,
                query.UserId);

            // Get unique room IDs from the reservations
            var roomIds = reservations
                .Select(r => r.RoomId)
                .Distinct()
                .ToList();

            // Fetch room names for the unique room IDs
            var roomNames = new Dictionary<Guid, string>();
            foreach (var roomId in roomIds)
            {
                var room = await _roomRepository.GetByIdAsync(roomId);
                if (room != null)
                {
                    roomNames[roomId] = room.Name;
                }
            }

            // Create calendar view DTO
            var calendarViewDto = new CalendarViewDto
            {
                StartDate = startDate,
                EndDate = endDate,
                RoomIds = roomIds,
                RoomNames = roomNames,
                Reservations = reservations.Select(r => new CalendarReservationDto
                {
                    Id = r.Id,
                    Title = r.Purpose ?? "Untitled Reservation",
                    Purpose = r.Purpose,
                    Description = r.Notes,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    RoomId = r.RoomId,
                    RoomName = r.Room?.Name ?? "Unknown Room",
                    UserId = r.UserId,
                    UserName = r.User?.Name ?? "Unknown User",
                    Status = r.Status,
                    IsCurrentUserReservation = currentUserId.HasValue && r.UserId == currentUserId.Value
                }).ToList()
            };

            return calendarViewDto;
        }

        private (DateTime StartDate, DateTime EndDate) AdjustDatesForViewType(
            DateTime requestedStart, 
            DateTime requestedEnd, 
            CalendarViewType viewType)
        {
            // If end date is before start date, swap them
            if (requestedEnd < requestedStart)
            {
                var temp = requestedStart;
                requestedStart = requestedEnd;
                requestedEnd = temp;
            }

            // Apply default ranges based on view type if dates are too close or too far apart
            switch (viewType)
            {
                case CalendarViewType.Day:
                    // For day view, if the range is not exactly 1 day, adjust it
                    if ((requestedEnd - requestedStart).TotalDays != 1)
                    {
                        return (
                            requestedStart.Date,
                            requestedStart.Date.AddDays(1)
                        );
                    }
                    break;

                case CalendarViewType.Week:
                    // For week view, ensure the range is 7 days
                    if ((requestedEnd - requestedStart).TotalDays < 6.5 || (requestedEnd - requestedStart).TotalDays > 7.5)
                    {
                        // Get the Monday of the week containing the start date
                        var monday = requestedStart.Date.AddDays(-(int)requestedStart.DayOfWeek + 1);
                        if (requestedStart.DayOfWeek == DayOfWeek.Sunday)
                        {
                            monday = monday.AddDays(-7); // Previous week's Monday
                        }
                        
                        return (
                            monday,
                            monday.AddDays(7)
                        );
                    }
                    break;

                case CalendarViewType.Month:
                    // For month view, ensure the range covers a calendar month
                    if ((requestedEnd - requestedStart).TotalDays < 28 || (requestedEnd - requestedStart).TotalDays > 32)
                    {
                        // Get the first day of the month
                        var firstDayOfMonth = new DateTime(requestedStart.Year, requestedStart.Month, 1);
                        
                        return (
                            firstDayOfMonth,
                            firstDayOfMonth.AddMonths(1)
                        );
                    }
                    break;
            }

            return (requestedStart, requestedEnd);
        }
    }
} 