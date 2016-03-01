using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Interview.Models;
using Microsoft.Security.Application;

namespace Interview.Repositories
{

    /// <summary>
    /// Repository for Comment.
    /// </summary>
    public class CommentRepository : ICommentRepository, IDisposable
    {

        /// <summary>
        /// Application's DbContext
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();

        private bool disposed = false;

        /// <summary>
        /// Get comment by its ID.
        /// </summary>
        /// <param name="id">The comment's ID.</param>
        /// <returns>Returns a comment.</returns>
        public Comment GetCommentById(int? id)
        {
            return db.Comments.SingleOrDefault(c=>c.CommentID== id);
        }

        /// <summary>
        /// Get comments by user's username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns a list of comments.</returns>
        public IEnumerable<Comment> GetCommentsByUser(string username)
        {
            var comments = db.Comments.Where(u => u.User.UserName == username).ToList();
            return comments;
        }

        /// <summary>
        /// Add a comment to the database.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public void AddComment(Comment comment)
        {
            db.Comments.Add(comment);
            db.SaveChanges();
        }

        /// <summary>
        /// Remove a comment from the database.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public void DeleteComment(Comment comment)
        {
            db.Comments.Remove(comment);
            db.SaveChanges();
        }

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        public void SaveChanges()
        {
            db.SaveChanges();
        }

        /// <summary>
        /// Update a comment from the database.
        /// </summary>
        /// <param name="comment">The comment.</param>
        public void UpdateComment(Comment comment)
        {
            comment.CommentContent = Sanitizer.GetSafeHtmlFragment(comment.CommentContent);
            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Add vote to a comment.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void AddCommentVote(CommentVote vote)
        {
            db.CommentVotes.Add(vote);
            db.SaveChanges();
        }

        /// <summary>
        /// Update a comment's vote.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void UpdateCommentVote(CommentVote vote)
        {
            db.Entry(vote).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Delete a comment's vote.
        /// </summary>
        /// <param name="vote"></param>
        public void DeleteCommentVote(CommentVote vote)
        {
            db.CommentVotes.Remove(vote);
            db.SaveChanges();
        }

        /// <summary>
        /// Add a flag to a comment.
        /// </summary>
        /// <param name="flag">The flag.</param>
        public void AddCommentFlag(CommentFlag flag)
        {
            db.CommentFlags.Add(flag);
            db.SaveChanges();
        }

        /// <summary>
        /// Update a comment's flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        public void UpdateCommentFlag(CommentFlag flag)
        {
            db.Entry(flag).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Delete a flag from a comment.
        /// </summary>
        /// <param name="flag">The flag.</param>
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