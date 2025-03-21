using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workshop_api_greenfield.Application.DTOs;
using Workshop_api_greenfield.Application.Services;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.API.Controllers
{
    /// <summary>
    /// API controller for managing rooms.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomsController"/> class.
        /// </summary>
        /// <param name="roomRepository">The room repository.</param>
        public RoomsController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>A collection of all rooms.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoomDto>), 200)]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomRepository.GetAllAsync();
            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Description = r.Description,
                Floor = r.Floor,
                Features = r.Features.ToList()
            });
            
            return Ok(roomDtos);
        }

        /// <summary>
        /// Gets a room by its ID.
        /// </summary>
        /// <param name="id">The ID of the room to get.</param>
        /// <returns>The room with the specified ID.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoomDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            
            if (room == null)
            {
                return NotFound();
            }

            var roomDto = new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                Capacity = room.Capacity,
                Description = room.Description,
                Floor = room.Floor,
                Features = room.Features.ToList()
            };

            return Ok(roomDto);
        }

        /// <summary>
        /// Gets available rooms for a specific time slot.
        /// </summary>
        /// <param name="startTime">The start time of the time slot.</param>
        /// <param name="endTime">The end time of the time slot.</param>
        /// <returns>A collection of available rooms for the specified time slot.</returns>
        [HttpGet("available")]
        [ProducesResponseType(typeof(IEnumerable<RoomDto>), 200)]
        [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
        {
            if (startTime >= endTime)
            {
                ModelState.AddModelError("TimeSlot", "End time must be after start time.");
                return ValidationProblem(ModelState);
            }

            var rooms = await _roomRepository.GetAvailableRoomsAsync(startTime, endTime);
            var roomDtos = rooms.Select(r => new RoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Description = r.Description,
                Floor = r.Floor,
                Features = r.Features.ToList()
            });
            
            return Ok(roomDtos);
        }
    }
} 