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
    /// Author - Hieu Nguyen & Long Nguyen
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
        /// Get all the flagged/reported comments.
        /// </summary>
        /// <returns>All flagged comments.</returns>
        public IEnumerable<Comment> GetFlaggedComments()
        {
            return db.Comments.Where(c => c.FlagPoint < 0).OrderBy(c => c.FlagPoint).ToList();
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
        /// <summary>
        /// Process the vote for the given comment Id.
        /// </summary>
        /// <param name="voteStatus">The vote status, either 1 or -1</param>
        /// <param name="commentId">The comment Id for the voted comment</param>
        /// <param name="userId">User id of the user that voted</param>
        /// <returns>The processed vote score of the comment</returns>
        public int ProcessCommentVote(int voteStatus, int commentId, string userId)
        {
            Comment comment = GetCommentById(commentId);
            CommentVote userOriginalVote = null;
            // check if the user already voted, and retrieve that vote if voted
            foreach (var vote in comment.VoteList)
            {
                if (vote.VoteUserId == userId)
                {
                    userOriginalVote = vote;
                    break;
                }
            }
            if (userOriginalVote == null) // user haven't voted
            {
                CommentVote newVote = new CommentVote
                {
                    VoteUserId = userId,
                    VoteStatus = voteStatus,
                    CommentID = commentId
                };
                AddCommentVote(newVote);
                comment.CurrentVote += voteStatus;
            }
            else // user voted
            {
                int originalVoteStatus = userOriginalVote.VoteStatus;
                if (voteStatus == 1 && originalVoteStatus == 1) // cancel upvote
                {
                    DeleteCommentVote(userOriginalVote);
                    comment.CurrentVote--;
                }
                else if (voteStatus == 1 && originalVoteStatus == -1) // switch to upvote
                {
                    userOriginalVote.VoteStatus = 1;
                    UpdateCommentVote(userOriginalVote);
                    comment.CurrentVote += 2;

                }
                else if (voteStatus == -1 && originalVoteStatus == 1) // switch to downvote
                {
                    userOriginalVote.VoteStatus = -1;
                    UpdateCommentVote(userOriginalVote);
                    comment.CurrentVote -= 2;
                }
                else if (voteStatus == -1 && originalVoteStatus == -1) // cancel downvote
                {
                    DeleteCommentVote(userOriginalVote);
                    comment.CurrentVote++;
                }

            }
            UpdateComment(comment);
            return comment.CurrentVote;
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
        /// Process the comment flag by either flag or unflag the comment for the given
        /// user id.
        /// </summary>
        /// <param name="commentId">The comment id of the comment to process.</param>
        /// <param name="userId">The user id of user that flag/unflag.</param>
        /// <returns>Current flag status, either 1 for unflag or -1 for flag</returns>
        public int ProcessCommentFlag(int commentId, string userId)
        {
            Comment comment = GetCommentById(commentId);
            foreach (CommentFlag flag in comment.CommentFlags)
            {
                if (flag.FlaggedUserId == userId)
                {
                    DeleteCommentFlag(flag);
                    comment.FlagPoint++;
                    UpdateComment(comment);
                    return 1;
                }
            }
            CommentFlag commentFlag = new CommentFlag
            {
                FlaggedUserId = userId,
                CommentID = comment.CommentID
            };
            AddCommentFlag(commentFlag);
            comment.FlagPoint--;
            UpdateComment(comment);
            return -1;
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