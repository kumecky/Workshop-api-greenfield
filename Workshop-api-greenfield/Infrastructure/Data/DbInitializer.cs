using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Infrastructure.Data
{
    /// <summary>
    /// Service for initializing the database with mock data.
    /// </summary>
    public static class DbInitializer
    {
        /// <summary>
        /// Initializes the database with mock data.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="logger">The logger.</param>
        public static async Task InitializeAsync<T>(IServiceProvider serviceProvider, ILogger<T> logger)
        {
            try
            {
                logger.LogInformation("Starting database initialization...");
                
                // Get a scoped service provider
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();

                await dbContext.Database.EnsureCreatedAsync();

                // Clear existing data
                logger.LogInformation("Clearing existing data from database...");
                dbContext.Reservations.RemoveRange(await dbContext.Reservations.ToListAsync());
                dbContext.Users.RemoveRange(await dbContext.Users.ToListAsync());
                dbContext.Rooms.RemoveRange(await dbContext.Rooms.ToListAsync());
                await dbContext.SaveChangesAsync();
                logger.LogInformation("Existing data cleared successfully.");
                
                // Seed new data
                await SeedRoomsAsync(dbContext);
                logger.LogInformation("Rooms seeded successfully.");

                await SeedUsersAsync(dbContext);
                logger.LogInformation("Users seeded successfully.");

                await SeedReservationsAsync(dbContext);
                logger.LogInformation("Reservations seeded successfully.");

                logger.LogInformation("Database initialization completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }

        private static async Task SeedRoomsAsync(ReservationDbContext dbContext)
        {
            // Create rooms with features
            var rooms = new List<Room>
            {
                new Room
                {
                    Name = "Executive Meeting Room",
                    Capacity = 30,
                    Description = "Large conference room with executive amenities",
                    Floor = 1,
                    Features = new List<RoomFeature>
                    {
                        RoomFeature.Projector,
                        RoomFeature.VideoConference,
                        RoomFeature.Whiteboard,
                        RoomFeature.AirConditioning,
                        RoomFeature.TVScreen
                    }
                },
                new Room
                {
                    Name = "Development Team Room",
                    Capacity = 12,
                    Description = "Medium-sized room for development team meetings",
                    Floor = 2,
                    Features = new List<RoomFeature>
                    {
                        RoomFeature.Whiteboard,
                        RoomFeature.TVScreen,
                        RoomFeature.Computer
                    }
                },
                new Room
                {
                    Name = "Quick Huddle Room",
                    Capacity = 4,
                    Description = "Small room for quick meetings or calls",
                    Floor = 2,
                    Features = new List<RoomFeature>
                    {
                        RoomFeature.Whiteboard,
                        RoomFeature.WheelchairAccessible
                    }
                },
                new Room
                {
                    Name = "Client Presentation Room",
                    Capacity = 20,
                    Description = "Room designed for client presentations",
                    Floor = 3,
                    Features = new List<RoomFeature>
                    {
                        RoomFeature.Projector,
                        RoomFeature.TVScreen,
                        RoomFeature.VideoConference,
                        RoomFeature.AirConditioning,
                        RoomFeature.WheelchairAccessible
                    }
                }
            };

            dbContext.Rooms.AddRange(rooms);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedUsersAsync(ReservationDbContext dbContext)
        {
            var users = new List<User>
            {
                new User
                {
                    Name = "John Smith",
                    Email = "john.smith@company.com",
                    Department = "Engineering"
                },
                new User
                {
                    Name = "Emily Johnson",
                    Email = "emily.johnson@company.com",
                    Department = "Marketing"
                },
                new User
                {
                    Name = "Michael Brown",
                    Email = "michael.brown@company.com",
                    Department = "Human Resources"
                },
                new User
                {
                    Name = "Sarah Davis",
                    Email = "sarah.davis@company.com",
                    Department = "Sales"
                },
                new User
                {
                    Name = "Robert Wilson",
                    Email = "robert.wilson@company.com",
                    Department = "Finance"
                }
            };

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedReservationsAsync(ReservationDbContext dbContext)
        {
            // Get rooms and users from the database
            var rooms = await dbContext.Rooms.ToListAsync();
            var users = await dbContext.Users.ToListAsync();

            if (!rooms.Any() || !users.Any())
            {
                return; // Can't seed reservations without rooms and users
            }

            // Get reference date (current date)
            var referenceDate = DateTime.Now.Date;
            
            // Log the reference date for debugging
            Console.WriteLine($"Seeding reservations with reference date: {referenceDate:yyyy-MM-dd}");

            // Create list for all reservations
            var reservations = new List<Reservation>();

            // Add reservations for today
            var todayReservations = new List<Reservation>
            {
                new Reservation
                {
                    RoomId = rooms[0].Id,
                    UserId = users[0].Id,
                    Purpose = "Daily Stand-up",
                    Notes = "Quick meeting to discuss progress",
                    StartTime = referenceDate.AddHours(9),
                    EndTime = referenceDate.AddHours(9).AddMinutes(30),
                    Status = ReservationStatus.Confirmed
                },
                new Reservation
                {
                    RoomId = rooms[1].Id,
                    UserId = users[1].Id,
                    Purpose = "Client Demo",
                    Notes = "Presenting new features to client",
                    StartTime = referenceDate.AddHours(14),
                    EndTime = referenceDate.AddHours(15),
                    Status = ReservationStatus.Confirmed
                },
                new Reservation
                {
                    RoomId = rooms[2].Id,
                    UserId = users[2].Id,
                    Purpose = "One-on-One Meeting",
                    Notes = "Performance review",
                    StartTime = referenceDate.AddHours(11),
                    EndTime = referenceDate.AddHours(12),
                    Status = ReservationStatus.Confirmed
                }
            };
            reservations.AddRange(todayReservations);

            // Add reservations for this week (1 day ahead)
            var tomorrowDate = referenceDate.AddDays(1);
            var tomorrowReservations = new List<Reservation>
            {
                new Reservation
                {
                    RoomId = rooms[0].Id,
                    UserId = users[3].Id,
                    Purpose = "Sprint Planning",
                    Notes = "Planning next sprint work",
                    StartTime = tomorrowDate.AddHours(10),
                    EndTime = tomorrowDate.AddHours(12),
                    Status = ReservationStatus.Confirmed
                },
                new Reservation
                {
                    RoomId = rooms[3].Id,
                    UserId = users[4].Id,
                    Purpose = "Team Lunch",
                    Notes = "Team building",
                    StartTime = tomorrowDate.AddHours(12),
                    EndTime = tomorrowDate.AddHours(13),
                    Status = ReservationStatus.Confirmed
                }
            };
            reservations.AddRange(tomorrowReservations);
            
            // Add reservations for yesterday
            var yesterdayDate = referenceDate.AddDays(-1);
            var yesterdayReservations = new List<Reservation>
            {
                new Reservation
                {
                    RoomId = rooms[1].Id,
                    UserId = users[2].Id,
                    Purpose = "Sprint Review",
                    Notes = "Review of sprint accomplishments",
                    StartTime = yesterdayDate.AddHours(15),
                    EndTime = yesterdayDate.AddHours(16),
                    Status = ReservationStatus.Confirmed
                }
            };
            reservations.AddRange(yesterdayReservations);
            
            // Add a reservation for 2 days ahead
            var twoDaysAheadDate = referenceDate.AddDays(2);
            var futureMeetings = new List<Reservation>
            {
                new Reservation
                {
                    RoomId = rooms[2].Id,
                    UserId = users[0].Id,
                    Purpose = "Technical Design Review",
                    Notes = "Discussing architecture changes",
                    StartTime = twoDaysAheadDate.AddHours(13),
                    EndTime = twoDaysAheadDate.AddHours(14).AddMinutes(30),
                    Status = ReservationStatus.Confirmed
                },
                new Reservation
                {
                    RoomId = rooms[3].Id,
                    UserId = users[1].Id,
                    Purpose = "Quarterly Review",
                    Notes = "Quarterly business review",
                    StartTime = twoDaysAheadDate.AddHours(10),
                    EndTime = twoDaysAheadDate.AddHours(11),
                    Status = ReservationStatus.Pending
                }
            };
            reservations.AddRange(futureMeetings);

            // Log the details of the reservations being created
            foreach (var res in reservations)
            {
                Console.WriteLine($"Creating reservation: {res.Purpose} in Room: {rooms.First(r => r.Id == res.RoomId).Name}, " +
                                 $"Start: {res.StartTime:yyyy-MM-dd HH:mm}, End: {res.EndTime:yyyy-MM-dd HH:mm}");
            }

            // Add all reservations to the database
            dbContext.Reservations.AddRange(reservations);
            await dbContext.SaveChangesAsync();
            
            Console.WriteLine($"Added {reservations.Count} reservations to the database");
        }
    }
} 