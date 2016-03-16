using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interview.Models;
using System.Web.Security;
using Interview.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Interview.Repositories
{

    /// <summary>
    /// Repository for Admin controller.
    /// Author - Hieu Nguyen & Long Nguyen
    /// </summary>
    public class AdminRepository : IAdminRepository
    {

        /// <summary>
        /// Application's DbContext.
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Add role to a specific user.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="userId">Id of an user.</param>
        public void AddRoleToUser(string role, string userId)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.AddToRole(userId, role);
        }

        /// <summary>
        /// Get all registered users.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

        /// <summary>
        /// Get user by the user's username.
        /// </summary>
        /// <param name="username">user's username.</param>
        /// <returns>Returns nn user.</returns>
        public ApplicationUser GetUserByUsername(string username)
        {
            return db.Users.SingleOrDefault(u => u.UserName == username);
        }

        /// <summary>
        /// Get all registered users who are not in the admin role.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        public IEnumerable<ApplicationUser> GetNonAdminUsers()
        {
            string adminRoleId = GetAdminRoleId();
            var admins = db.Users.Where(x => x.Roles.Select(y => y.RoleId)
                    .Contains(adminRoleId)).ToList();
            return db.Users.ToList().Except(admins).ToList();
        }

        /// <summary>
        /// Get all registered users who are in the admin role.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        public IEnumerable<ApplicationUser> GetAdminUsers()
        {
            string adminRoleId = GetAdminRoleId();
            return db.Users.Where(x => x.Roles.Select(y => y.RoleId)
                    .Contains(adminRoleId)).ToList();
        }

        /// <summary>
        /// Private helper. Get the admin role's ID.
        /// </summary>
        /// <returns>Returns the admin role's ID.</returns>
        private string GetAdminRoleId()
        {
            string adminRoleId = "";
            var roles = db.Roles.ToList();
            foreach (var role in roles)
            {
                if (role.Name == "Admin")
                {
                    adminRoleId = role.Id;
                }
            }
            return adminRoleId;
        }

        /// <summary>
        /// Remove the role from a specific user.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <param name="userId">The user's ID.</param>
        public void RemoveRoleFromUser(string role, string userId)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == userId);
            var userRoles = Roles.FindUsersInRole(ConstantHelper.AdminRole, user.UserName);
            if(userRoles.Length > 0)
            {
                Roles.RemoveUserFromRoles(user.UserName, userRoles);
            }
        }

    }
}