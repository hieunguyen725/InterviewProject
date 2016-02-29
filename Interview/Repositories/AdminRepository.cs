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
    public class AdminRepository : IAdminRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public void AddRoleToUser(string role, string userId)
        //{
        //    var user = db.Users.SingleOrDefault(u => u.Id == userId);
        //    var userRoles = Roles.FindUsersInRole(ConstantHelper.AdminRole, user.UserName);
        //    if (userRoles.Length <= 0)
        //    {
        //        Roles.AddUserToRole(user.UserName, ConstantHelper.AdminRole);
        //    }
        //}

        public void AddRoleToUser(string role, string userId)
        {
            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            userManager.AddToRole(userId, role);
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public ApplicationUser GetUserByUsername(string username)
        {
            return db.Users.SingleOrDefault(u => u.UserName == username);
        }

        public IEnumerable<ApplicationUser> GetNonAdminUsers()
        {
            string adminRoleId = GetAdminRoleId();
            var admins = db.Users.Where(x => x.Roles.Select(y => y.RoleId)
                    .Contains(adminRoleId)).ToList();
            return db.Users.ToList().Except(admins).ToList();
        }

        public IEnumerable<ApplicationUser> GetAdminUsers()
        {
            string adminRoleId = GetAdminRoleId();
            return db.Users.Where(x => x.Roles.Select(y => y.RoleId)
                    .Contains(adminRoleId)).ToList();
        }

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

        public IEnumerable<Post> GetAllPosts()
        {
            return db.Posts.ToList();
        }

        public IEnumerable<Post> GetFlaggedPosts()
        {
            return db.Posts.Where(p => p.FlagPoint < 0).OrderBy(p => p.FlagPoint).ToList();
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