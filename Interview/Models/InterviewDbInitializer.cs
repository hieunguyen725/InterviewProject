using Interview.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Interview.Models
{

    /// <summary>
    /// Database initializer.
    /// </summary>
    public class InterviewDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        // This is the total number of posts and users. Change it to any number you like.
        private int _num = 11;

        protected override void Seed(ApplicationDbContext context)
        {

            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // Create admin role if it doesn't exists
            if (!context.Roles.Any(r => r.Name == ConstantHelper.AdminRole))
            {
                var role = new IdentityRole { Name = ConstantHelper.AdminRole };
                roleManager.Create(role);
            }

            // Create user named admin1 if it doesn't exists
            if (!context.Users.Any(u => u.UserName == "admin1"))
            {
                var user = new ApplicationUser { UserName = "admin1", Email = "admin1@gmail.com" };

                userManager.Create(user, "Adminpass1");
                userManager.AddToRole(user.Id, ConstantHelper.AdminRole);
                UserProfile up = new UserProfile
                {
                    AboutMe = "admin1 is a superman",
                    Username = user.UserName,
                    UserId = user.Id
                };
                context.UserProfiles.Add(up);
            }

            // Create user named user1 if it doesn't exists
            if(!context.Users.Any(u=>u.UserName == "user1"))
            {
                var user = new ApplicationUser() { UserName = "user1", Email = "user1@gmail.com" };
                userManager.Create(user, "Userpass1");
                UserProfile up = new UserProfile
                {
                    AboutMe = "user1 is a superman",
                    Username = user.UserName,
                    UserId = user.Id
                };
                context.UserProfiles.Add(up);
            }

            var tags = new List<Tag>
            {
                new Tag{TagName = "Java"},
                new Tag{TagName = "C"},
                new Tag{TagName = "C++"},
                new Tag{TagName = "C#"},
                new Tag{TagName = "HTML"},
                new Tag{TagName = "CSS"},
                new Tag{TagName = "JavaScript"},
                new Tag{TagName = "Ruby"},
                new Tag{TagName = "Front-end"},
                new Tag{TagName = "Back-end"},
                new Tag{TagName = "Web-Development"},
                new Tag{TagName = "Algorithm"},
                new Tag{TagName = "Data-structure"},
                new Tag{TagName = "PHP"},
                new Tag{TagName = "Database"},
                new Tag{TagName = "AngularJs"},
                new Tag{TagName = "ReactJs"},
                new Tag{TagName = "RubyOnRails"},
                new Tag{TagName = "Laravel"},
                new Tag{TagName = "General"},
                new Tag {TagName = "Object Oriented Programming" },
                new Tag {TagName = "Mobile Development" },
                new Tag {TagName = "Operating System" },
                new Tag {TagName = "Problem Solving" }
            };
            foreach (var tag in tags)
            {
                context.Tags.Add(tag);
            }

            base.Seed(context);
        }
    }
}