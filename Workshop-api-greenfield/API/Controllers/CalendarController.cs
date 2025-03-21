using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Workshop_api_greenfield.Application.DTOs;
using Workshop_api_greenfield.Application.Services;
using Workshop_api_greenfield.Infrastructure.Data;

namespace Workshop_api_greenfield.API.Controllers
{
    /// <summary>
    /// API controller for managing calendar views and operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly CalendarService _calendarService;
        private readonly ILogger<CalendarController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarController"/> class.
        /// </summary>
        /// <param name="calendarService">The calendar service.</param>
        /// <param name="logger">The logger.</param>
        public CalendarController(CalendarService calendarService, ILogger<CalendarController> logger)
        {
            _calendarService = calendarService;
            _logger = logger;
        }

        /// <summary>
        /// Gets calendar data for the specified date range and view type.
        /// </summary>
        /// <param name="query">The calendar query parameters.</param>
        /// <returns>Calendar data including reservations and room information.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(CalendarViewDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> GetCalendarData([FromQuery] CalendarQueryDto query)
        {
            _logger.LogInformation($"Getting calendar data from {query.StartDate:yyyy-MM-dd} to {query.EndDate:yyyy-MM-dd}");
            
            // In a real application, you would get the current user ID from authentication
            Guid? currentUserId = null; // TODO: Get from auth context when implemented

            var calendarData = await _calendarService.GetCalendarDataAsync(query, currentUserId);
            
            _logger.LogInformation($"Found {calendarData.Reservations.Count} reservations in the calendar view");
            
            return Ok(calendarData);
        }

        /// <summary>
        /// Gets calendar data for the default time range (current week).
        /// </summary>
        /// <returns>Calendar data for the current week.</returns>
        [HttpGet("current-week")]
        [ProducesResponseType(typeof(CalendarViewDto), 200)]
        public async Task<IActionResult> GetCurrentWeekCalendar()
        {
            var today = DateTime.Today;
            var monday = today.AddDays(-(int)today.DayOfWeek + 1);
            if (today.DayOfWeek == DayOfWeek.Sunday)
            {
                monday = monday.AddDays(-7); // Previous week's Monday
            }

            _logger.LogInformation($"Getting current week calendar from {monday:yyyy-MM-dd} to {monday.AddDays(7):yyyy-MM-dd}");

            var query = new CalendarQueryDto
            {
                StartDate = monday,
                EndDate = monday.AddDays(7),
                ViewType = CalendarViewType.Week
            };

            // In a real application, you would get the current user ID from authentication
            Guid? currentUserId = null; // TODO: Get from auth context when implemented

            var calendarData = await _calendarService.GetCalendarDataAsync(query, currentUserId);
            
            _logger.LogInformation($"Found {calendarData.Reservations.Count} reservations for current week");
            
            return Ok(calendarData);
        }

        /// <summary>
        /// Gets calendar data for the current month.
        /// </summary>
        /// <returns>Calendar data for the current month.</returns>
        [HttpGet("current-month")]
        [ProducesResponseType(typeof(CalendarViewDto), 200)]
        public async Task<IActionResult> GetCurrentMonthCalendar()
        {
            var today = DateTime.Today;
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            _logger.LogInformation($"Getting current month calendar from {firstDayOfMonth:yyyy-MM-dd} to {firstDayOfNextMonth:yyyy-MM-dd}");

            var query = new CalendarQueryDto
            {
                StartDate = firstDayOfMonth,
                EndDate = firstDayOfNextMonth,
                ViewType = CalendarViewType.Month
            };

            // In a real application, you would get the current user ID from authentication
            Guid? currentUserId = null; // TODO: Get from auth context when implemented

            var calendarData = await _calendarService.GetCalendarDataAsync(query, currentUserId);
            
            _logger.LogInformation($"Found {calendarData.Reservations.Count} reservations for current month");
            
            return Ok(calendarData);
        }

        /// <summary>
        /// Gets calendar data for a specific room.
        /// </summary>
        /// <param name="roomId">The ID of the room to get calendar data for.</param>
        /// <param name="startDate">The start date of the range (defaults to today).</param>
        /// <param name="viewType">The view type (defaults to Week).</param>
        /// <returns>Calendar data for the specified room.</returns>
        [HttpGet("room/{roomId}")]
        [ProducesResponseType(typeof(CalendarViewDto), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> GetRoomCalendar(
            Guid roomId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] CalendarViewType viewType = CalendarViewType.Week)
        {
            _logger.LogInformation($"Getting calendar for room {roomId}");
            
            var actualStartDate = startDate ?? DateTime.Today;
            DateTime endDate;

            // Calculate the end date based on the view type
            switch (viewType)
            {
                case CalendarViewType.Day:
                    endDate = actualStartDate.AddDays(1);
                    break;
                case CalendarViewType.Week:
                    // Find the Monday of the week containing the start date
                    var dayOfWeek = (int)actualStartDate.DayOfWeek;
                    var monday = actualStartDate.AddDays(dayOfWeek == 0 ? -6 : 1 - dayOfWeek);
                    actualStartDate = monday;
                    endDate = monday.AddDays(7);
                    break;
                case CalendarViewType.Month:
                    actualStartDate = new DateTime(actualStartDate.Year, actualStartDate.Month, 1);
                    endDate = actualStartDate.AddMonths(1);
                    break;
                default:
                    endDate = actualStartDate.AddDays(7);
                    break;
            }
            
            _logger.LogInformation($"Date range for room calendar: {actualStartDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");

            var query = new CalendarQueryDto
            {
                StartDate = actualStartDate,
                EndDate = endDate,
                ViewType = viewType,
                RoomIds = new System.Collections.Generic.List<Guid> { roomId }
            };

            // In a real application, you would get the current user ID from authentication
            Guid? currentUserId = null; // TODO: Get from auth context when implemented

            var calendarData = await _calendarService.GetCalendarDataAsync(query, currentUserId);
            
            _logger.LogInformation($"Found {calendarData.Reservations.Count} reservations for room {roomId}");
            
            return Ok(calendarData);
        }

        // Debug endpoint to re-seed the database (only for development)
        [HttpPost("debug/reset-data")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ResetData()
        {
            // Only allow reseeding from localhost during development
            if (!HttpContext.Request.Host.Host.Contains("localhost") && 
                !HttpContext.Request.Host.Host.Contains("127.0.0.1"))
            {
                return Forbid("This endpoint is only available in development environment");
            }

            _logger.LogWarning("Database reset initiated from debug endpoint");
            
            try
            {
                await DbInitializer.InitializeAsync(HttpContext.RequestServices, _logger);
                return Ok(new { message = "Database has been reset successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting database from debug endpoint");
                return StatusCode(500, new { message = $"Error resetting database: {ex.Message}" });
            }
        }
    }
} 