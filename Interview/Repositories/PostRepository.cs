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

        public IEnumerable<Post> GetAllPosts()
        {
            return db.Posts.Include(c => c.Comments).Include(u => u.User).ToList();
        }

        public IEnumerable<Post> GetLatestPosts()
        {
            return db.Posts.OrderByDescending(p => p.CreatedAt).Take(10).Include(c => c.Comments).Include(u => u.User).ToList();
        }

        public Post GetPostById(int? id)
        {
            return db.Posts.Include(c => c.Comments).Include(u => u.User).SingleOrDefault(p => p.PostID == id);
        }

        public IEnumerable<Post> GetPostByUser(string userId)
        {
            return db.Posts.Where(p => p.UserID == userId).Include(c => c.Comments).Include(u => u.User).ToList();
        }

        public IEnumerable<Post> GetPostByUserName(string username)
        {
            return db.Posts.Where(p => p.User.UserName == username).Include(c => c.Comments).Include(u => u.User).ToList();
        }

        public IEnumerable<Post> GetPostByCategory(string category)
        {
            if (category == "All")
            {
                return db.Posts.Include(c => c.Comments).Include(u => u.User).ToList();
            }
            else
            {
                return db.Posts.Where(p => p.SelectedCategory == category).Include(c => c.Comments).Include(u => u.User).ToList();
            }
        }

        public IEnumerable<Post> GetPostBySearch(string search)
        {
            return db.Posts.Where(p => p.PostTitle.Contains(search) || p.PostContent.Contains(search)).Include(c => c.Comments).Include(u => u.User).ToList();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void UpdatePost(Post post)
        {
            post.PostContent = Sanitizer.GetSafeHtmlFragment(post.PostContent);
            db.Entry(post).State = System.Data.Entity.EntityState.Modified;
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