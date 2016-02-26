using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class InterviewDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        // This is the total number of posts and users. Change it to any number you like.
        private int _num = 11;

        protected override void Seed(ApplicationDbContext context)
        {

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
                new Tag{TagName = "front-end"},
                new Tag{TagName = "back-end"},
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
            };
            foreach (var tag in tags)
            {
                context.Tags.Add(tag);
            }


            //var ph = new PasswordHasher();
            //for (int i = 0; i < _num; i++)
            //{
            //    string pass = ph.HashPassword("Password" + i);
            //    context.Users.Add(new ApplicationUser
            //    {
            //        UserName = "user" + i,
            //        Email = "user" + i + "@yahoo.com",
            //        PasswordHash = pass
            //    });
            //}
            
            //Random random = new Random();

            //for (int i = 0; i < _num; i++)
            //{
            //    int randomNum = random.Next(0, 8);
            //    int randomUser = random.Next(0, _num);
            //    int randomViewCount = random.Next(0, 1000);
            //    context.Posts.Add(new Post
            //    {
            //        PostTitle = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.............. #" + i,
            //        PostContent = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
            //        CreatedAt = DateTime.Now,
            //        ViewCount = randomViewCount,
            //        UserID = i.ToString()
            //    });
            //}

            base.Seed(context);
        }
    }
}