using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interview.Models;
using System.Data.Entity;

namespace Interview.Repositories
{

    /// <summary>
    /// Repository for User.
    /// Author - Hieu Nguyen & Long Nguyen
    /// </summary>
    public class UserRepository : IUserRepository
    {

        /// <summary>
        /// Application's DbContext.
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Get all the users.
        /// </summary>
        /// <returns>Returns a list of users.</returns>
        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

        /// <summary>
        /// Get user by the user's ID.
        /// </summary>
        /// <param name="id">The user's ID.</param>
        /// <returns>Returns an user.</returns>
        public ApplicationUser GetUserById(string id)
        {
            return db.Users.Find(id);
        }

        /// <summary>
        /// Get user by the user's username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns an user.</returns>
        public ApplicationUser GetUserByUsername(string username)
        {
            return db.Users.SingleOrDefault(u => u.UserName == username);
        }

        /// <summary>
        /// Update a user's profile.
        /// </summary>
        /// <param name="UserProfile">The UserProfile.</param>
        public void AddUserProfile(UserProfile UserProfile)
        {
            db.UserProfiles.Add(UserProfile);
            db.SaveChanges();
        }

        /// <summary>
        /// Get an user's profile.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns a user's profile.</returns>
        public UserProfile GetUserProfile(string username)
        {
            return db.UserProfiles.SingleOrDefault(u => u.Username == username);
        }

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        /// <summary>
        /// Update a user's profile.
        /// </summary>
        /// <param name="user">The UserProfile.</param>
        public void UpdateUserProfile(UserProfile user)
        {

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}