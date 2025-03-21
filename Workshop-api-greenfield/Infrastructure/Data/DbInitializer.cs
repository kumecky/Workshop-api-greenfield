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

                // Only seed data if the database is empty
                if (!await dbContext.Rooms.AnyAsync())
                {
                    await SeedRoomsAsync(dbContext);
                    logger.LogInformation("Rooms seeded successfully.");
                }

                if (!await dbContext.Users.AnyAsync())
                {
                    await SeedUsersAsync(dbContext);
                    logger.LogInformation("Users seeded successfully.");
                }

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
    }
} 