using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Repositories
{
    public interface ICommentRepository
    {

        Comment GetCommentById(int? id);

        IEnumerable<Comment> GetCommentsByUser(string username);

        void AddComment(Comment comment);

        void UpdateComment(Comment comment);

        void DeleteComment(Comment comment);

        void AddCommentVote(CommentVote comment);

        void UpdateCommentVote(CommentVote comment);

        void DeleteCommentVote(CommentVote comment);

        void SaveChanges();
        
    }
}