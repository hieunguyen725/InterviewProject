using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Interview.Models;
using Microsoft.Security.Application;

namespace Interview.Repositories
{
    public class PostRepository : IPostRepository, IDisposable
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private bool disposed = false;

        public void AddPost(Post post)
        {
            db.Posts.Add(post);
            db.SaveChanges();        
        }

        public void DeletePost(Post post)
        {
            db.Posts.Remove(post);
            db.SaveChanges();
        }

        public IEnumerable<Tag> GetTags() {
            return db.Tags.Include(p => p.Posts).ToList();
        }

        public void AddPostToTags(Post post, string tagsS)
        {

            List<string> tags = tagsS.Split(',').ToList();

            for(int i = 0; i < tags.Count(); i++)
            {
                var tagName = tags.ElementAt(i);
                Tag temp = db.Tags.Include(p => p.Posts)
                                .SingleOrDefault(t => t.TagName == tagName);
                temp.Posts.Add(post);
            }
            db.SaveChanges();
         
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return db.Posts.Include(c => c.Comments)
                            .Include(u => u.User)
                            .Include(t=>t.Tags)
                            .Include(v => v.VoteList)
                            .ToList();
        }

        public IEnumerable<Post> GetLatestPosts()
        {
            return db.Posts.OrderByDescending(p => p.CreatedAt)
                            .Take(5).Include(c => c.Comments)
                            .Include(u => u.User).Include(t => t.Tags)
                            .Include(v => v.VoteList)
                            .ToList();
        }

        public Post GetPostById(int? id)
        {
            return db.Posts.Include(c => c.Comments)
                            .Include(u => u.User)
                            .Include(t => t.Tags)
                            .Include(v => v.VoteList)
                            .SingleOrDefault(p => p.PostID == id);
        }

        public IEnumerable<Post> GetPostByUser(string userId)
        {
            return db.Posts.Where(p => p.UserID == userId)
                        .Include(c => c.Comments)
                        .Include(u => u.User)
                        .Include(v => v.VoteList)
                        .Include(t => t.Tags).ToList();
        }

        public IEnumerable<Post> GetPostByUserName(string username)
        {
            return db.Posts.Where(p => p.User.UserName == username)
                        .Include(c => c.Comments)
                        .Include(t => t.Tags)
                        .Include(u => u.User)
                        .Include(v => v.VoteList)
                        .ToList();
        }

        public IEnumerable<Post> GetPostBySearch(string search)
        {
            return db.Posts.Where(p => p.PostTitle.Contains(search) || p.PostContent.Contains(search))
                        .Include(c => c.Comments)
                        .Include(u => u.User)
                        .Include(t => t.Tags)
                        .Include(v => v.VoteList)
                        .ToList();
        }

        public void AddPostVote(PostVote vote)
        {
            db.PostVotes.Add(vote);
            db.SaveChanges();
        }

        public void UpdatePostVote(PostVote vote)
        {
            db.Entry(vote).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void DeletePostVote(PostVote vote)
        {
            db.PostVotes.Remove(vote);
            db.SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void UpdatePost(Post post)
        {
            post.PostContent = Sanitizer.GetSafeHtmlFragment(post.PostContent);
            db.Entry(post).State = EntityState.Modified;
            db.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}