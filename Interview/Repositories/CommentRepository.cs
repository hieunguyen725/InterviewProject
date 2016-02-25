using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Interview.Models;
using Microsoft.Security.Application;

namespace Interview.Repositories
{
    public class CommentRepository : ICommentRepository, IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private bool disposed = false;

        public Comment GetCommentById(int? id)
        {
            return db.Comments.Include(p => p.Post).Include(u=>u.User).SingleOrDefault(c=>c.CommentID== id);
        }

        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
            db.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            db.Comments.Remove(comment);
            db.SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void UpdateComment(Comment comment)
        {
            comment.CommentContent = Sanitizer.GetSafeHtmlFragment(comment.CommentContent);
            db.Entry(comment).State = EntityState.Modified;
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