using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Repositories
{
    public interface IAdminRepository
    {
        IEnumerable<ApplicationUser> GetAllUsers();

        IEnumerable<ApplicationUser> GetNonAdminUsers();

        IEnumerable<ApplicationUser> GetAdminUsers();

        ApplicationUser GetUserByUsername(string username);

        void AddRoleToUser(string role, string userId);

        void RemoveRoleFromUser(string role, string userId);

        IEnumerable<Post> GetFlaggedPosts();

        IEnumerable<Post> GetAllPosts();
    }
}
