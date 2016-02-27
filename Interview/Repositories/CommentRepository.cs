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
            return db.Comments.SingleOrDefault(c=>c.CommentID== id);
        }

        public IEnumerable<Comment> GetCommentsByUser(string username)
        {
            var comments = db.Comments.Where(u => u.User.UserName == username).ToList();
            return comments;
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

        public void AddCommentVote(CommentVote vote)
        {
            db.CommentVotes.Add(vote);
            db.SaveChanges();
        }

        public void UpdateCommentVote(CommentVote vote)
        {
            db.Entry(vote).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteCommentVote(CommentVote vote)
        {
            db.CommentVotes.Remove(vote);
            db.SaveChanges();
        }

        public void AddCommentFlag(CommentFlag flag)
        {
            db.CommentFlags.Add(flag);
            db.SaveChanges();
        }

        public void UpdateCommentFlag(CommentFlag flag)
        {
            db.Entry(flag).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void DeleteCommentFlag(CommentFlag flag)
        {
            db.CommentFlags.Remove(flag);
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