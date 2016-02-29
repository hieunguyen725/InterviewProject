using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Interview.Models;
using System.Web.Security;
using Interview.Infrastructure;

namespace Interview.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void AddRoleToUser(string role, string userId)
        {
            var user = db.Users.SingleOrDefault(u => u.Id == userId);
            var userRoles = Roles.FindUsersInRole(ConstantHelper.AdminRole, user.UserName);
            if (userRoles.Length <= 0)
            {
                Roles.AddUserToRole(user.UserName, ConstantHelper.AdminRole);
            }
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

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