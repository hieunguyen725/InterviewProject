using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Repositories
{
    public interface IUserRepository
    {

        IEnumerable<ApplicationUser> GetAllUsers();

        ApplicationUser GetUserById(string id);

        ApplicationUser GetUserByUsername(string username);

        void AddUserProfile(UserProfile userprofile);

        UserProfile GetUserProfile(string username);

        void UpdateUserProfile(UserProfile user);

        void SaveChanges();
    }
}