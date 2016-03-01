using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Repositories
{
    /// <summary>
    /// Repository for Admin controller.
    /// </summary>
    public interface IAdminRepository
    {

        /// <summary>
        /// Get all registered users.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        IEnumerable<ApplicationUser> GetAllUsers();

        /// <summary>
        /// Get all registered users who are not in the admin role.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        IEnumerable<ApplicationUser> GetNonAdminUsers();

        /// <summary>
        /// Get all registered users who are in the admin role.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        IEnumerable<ApplicationUser> GetAdminUsers();

        /// <summary>
        /// Get user by the user's username.
        /// </summary>
        /// <param name="username">user's username.</param>
        /// <returns>Returns nn user.</returns>
        ApplicationUser GetUserByUsername(string username);

        /// <summary>
        /// Add role to a specific user.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="userId">Id of an user.</param>
        void AddRoleToUser(string role, string userId);

        /// <summary>
        /// Remove the role from a specific user.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="userId">The user's ID.</param>
        void RemoveRoleFromUser(string role, string userId);
        
    }
}
