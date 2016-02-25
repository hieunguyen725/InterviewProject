using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class InterviewDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        private int _num = 15;
        protected override void Seed(ApplicationDbContext context)
        {

            var ph = new PasswordHasher();
            for(int i = 0; i < _num; i++)
            {
                string pass = ph.HashPassword("Password" + i);
                context.Users.Add(new ApplicationUser
                {
                    Id = i.ToString(),
                    UserName = "user"+i,
                    Email = "user" + i + "@yahoo.com",
                    PasswordHash = pass
                });
            }

            var categories = new List<string>
            {
                "Data Structure", "Algorithm", "Operating System",
                "Programming Fundamentals", "Mobile Development",
                "Web Development", "Database", "Other"
            };

            var comments = new List<Comment>();
            Random random = new Random();
            for (int i = 0; i < _num; i++)
            {
                int randomNum = random.Next(0, _num);
                comments.Add(new Comment
                {
                    CommentID = i,
                    CommentContent = "Comment #" + i + " for post #" + i,
                    CreatedAt = DateTime.Now,
                    PostID = i,
                    UserID = i.ToString()
                });
            }
            foreach(var comment in comments)
            {
                context.Comments.Add(comment);
            }

            for (int i = 0; i < _num; i++)
            {
                int randomNum = random.Next(0, 8);
                int randomUser = random.Next(0, _num);
                int randomViewCount = random.Next(0, 1000);
                context.Posts.Add(new Post
                {
                    PostID = i,
                    PostTitle = "This is post title #" + i,
                    PostContent = "This is a post content, and it is supposed to be longer. Also, this post content belong to post #" + i,
                    CreatedAt = DateTime.Now,
                    SelectedCategory = categories.ElementAt(randomNum),
                    ViewCount = randomViewCount,
                    UserID = i.ToString()
                });
            }

            base.Seed(context);
        }
    }
}