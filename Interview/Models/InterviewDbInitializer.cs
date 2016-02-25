using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class InterviewDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {

            //var answers = new List<PostAnswer>()
            //{
            //    new PostAnswer()
            //    {
            //        Answer = "A",
            //        CreatedAt = new DateTime()
            //    }
            //};

            //var posts = new List<Post>()
            //{
            //    new Post()
            //    {
            //        Name = "A",
            //        PostQuestion = "Question A",
            //        PostAnswers = 
            //    }
            //};


            base.Seed(context);
        }
    }
}