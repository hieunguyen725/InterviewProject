using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interview.Models;
using System.Data.Entity;

namespace Interview.Repositories
{
    public class UserRepository : IUserRepository
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public ApplicationUser GetUserById(string id)
        {
            return db.Users.Find(id);
        }

        public ApplicationUser GetUserByUsername(string username)
        {
            return db.Users.SingleOrDefault(u => u.UserName == username);
        }

        public void AddUserProfile(UserProfile UserProfile)
        {
            db.UserProfiles.Add(UserProfile);
            db.SaveChanges();
        }

        public UserProfile GetUserProfile(string username)
        {
            return db.UserProfiles.SingleOrDefault(u => u.Username == username);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void UpdateUserProfile(UserProfile user)
        {

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}