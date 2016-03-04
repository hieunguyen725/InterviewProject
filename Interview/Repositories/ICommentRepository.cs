using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Repositories
{

    /// <summary>
    /// Repository for Comment.
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Get comment by its ID.
        /// </summary>
        /// <param name="id">The comment's ID.</param>
        /// <returns>Returns a comment.</returns>
        Comment GetCommentById(int? id);

        /// <summary>
        /// Get comments by user's username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns a list of comments.</returns>
        IEnumerable<Comment> GetCommentsByUser(string username);

        /// <summary>
        /// Get all the flagged/reported comments.
        /// </summary>
        /// <returns>All flagged comments.</returns>
        IEnumerable<Comment> GetFlaggedComments();

        /// <summary>
        /// Add a comment to the database.
        /// </summary>
        /// <param name="comment">The comment.</param>
        void AddComment(Comment comment);

        /// <summary>
        /// Update a comment from the database.
        /// </summary>
        /// <param name="comment">The comment.</param>
        void UpdateComment(Comment comment);

        /// <summary>
        /// Remove a comment from the database.
        /// </summary>
        /// <param name="comment">The comment.</param>
        void DeleteComment(Comment comment);

        /// <summary>
        /// Add vote to a comment.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void AddCommentVote(CommentVote vote);

        /// <summary>
        /// Update a comment's vote.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void UpdateCommentVote(CommentVote vote);

        /// <summary>
        /// Delete a comment's vote.
        /// </summary>
        /// <param name="vote"></param>
        void DeleteCommentVote(CommentVote vote);

        /// <summary>
        /// Add a flag to a comment.
        /// </summary>
        /// <param name="flag">The flag.</param>
        void AddCommentFlag(CommentFlag flag);

        /// <summary>
        /// Update a comment's flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        void UpdateCommentFlag(CommentFlag flag);

        /// <summary>
        /// Delete a flag from a comment.
        /// </summary>
        /// <param name="flag">The flag.</param>
        void DeleteCommentFlag(CommentFlag flag);

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        void SaveChanges();
        
    }
}