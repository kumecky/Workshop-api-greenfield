using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workshop_api_greenfield.Domain.Entities;

namespace Workshop_api_greenfield.Application.Services
{
    /// <summary>
    /// Interface for the user repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>A collection of all users.</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to get.</param>
        /// <returns>The user with the specified ID, or null if not found.</returns>
        Task<User?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user to get.</param>
        /// <returns>The user with the specified email address, or null if not found.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Adds a new user.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>The added user.</returns>
        Task<User> AddAsync(User user);

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user to update.</param>
        /// <returns>The updated user.</returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>True if the user was deleted, false otherwise.</returns>
        Task<bool> DeleteAsync(Guid id);
    }
} 