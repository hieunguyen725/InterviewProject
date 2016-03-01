using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Repositories
{
    /// <summary>
    /// Repository for User.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get all the users.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        IEnumerable<ApplicationUser> GetAllUsers();

        /// <summary>
        /// Get user by the user's ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>Returns an user.</returns>
        ApplicationUser GetUserById(string id);

        /// <summary>
        /// Get user by the user's username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns an user.</returns>
        ApplicationUser GetUserByUsername(string username);

        /// <summary>
        /// Update a user's profile.
        /// </summary>
        /// <param name="UserProfile">The UserProfile.</param>
        void AddUserProfile(UserProfile userprofile);

        /// <summary>
        /// Get an user's profile.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns a user's profile.</returns>
        UserProfile GetUserProfile(string username);

        /// <summary>
        /// Update a user's profile.
        /// </summary>
        /// <param name="user">The UserProfile.</param>
        void UpdateUserProfile(UserProfile user);

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        void SaveChanges();
    }
}