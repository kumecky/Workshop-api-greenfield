using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Infrastructure.Data
{
    /// <summary>
    /// Database context for the room reservation application.
    /// </summary>
    public class ReservationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the context.</param>
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the rooms in the database.
        /// </summary>
        public DbSet<Room> Rooms { get; set; } = null!;

        /// <summary>
        /// Gets or sets the users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Gets or sets the reservations in the database.
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; } = null!;

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Room Entity Configuration
            modelBuilder.Entity<Room>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Room>()
                .Property(r => r.Name)
                .IsRequired();

            // User Entity Configuration
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            // Reservation Entity Configuration
            modelBuilder.Entity<Reservation>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Store enum as string
            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();

            // Configure RoomFeature list storage
            modelBuilder.Entity<Room>()
                .Property(r => r.Features)
                .HasConversion(
                    v => string.Join(',', v.Select(f => f.ToString())),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(s => Enum.Parse<RoomFeature>(s))
                         .ToList());

            // Comment out seed data as we're using the new DbInitializer
            // SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        // Leaving the SeedData method for reference
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Create Guids for the entities
            var room1Id = Guid.NewGuid();
            var room2Id = Guid.NewGuid();
            var room3Id = Guid.NewGuid();
            var user1Id = Guid.NewGuid();
            var user2Id = Guid.NewGuid();

            // Seed rooms
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = room1Id,
                    Name = "Conference Room A",
                    Capacity = 20,
                    Description = "Large conference room with projector and video conferencing",
                    Floor = 1
                },
                new Room
                {
                    Id = room2Id,
                    Name = "Meeting Room B",
                    Capacity = 8,
                    Description = "Medium-sized meeting room with whiteboard",
                    Floor = 2
                },
                new Room
                {
                    Id = room3Id,
                    Name = "Small Room C",
                    Capacity = 4,
                    Description = "Small meeting room for quick meetings",
                    Floor = 2
                }
            );

            // Entity Framework Core doesn't support seeding many-to-many relationships or collections directly
            // For production code, you would need to seed features in a different way
            // For now, we'll skip seeding the features as they're causing the issue

            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = user1Id,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Department = "IT"
                },
                new User
                {
                    Id = user2Id,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Department = "HR"
                }
            );
        }
    }
} 