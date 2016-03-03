using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Interview.Models;
using Microsoft.Security.Application;
using System.Data.Entity.Migrations;
using System.Diagnostics;

namespace Interview.Repositories
{

    /// <summary>
    /// Repository for Post.
    /// </summary>
    public class PostRepository : IPostRepository, IDisposable
    {

        /// <summary>
        /// Application's DbContext.
        /// </summary>
        private ApplicationDbContext db = new ApplicationDbContext();
        private bool disposed = false;

        /// <summary>
        /// Add a post to the database.
        /// </summary>
        /// <param name="post">The post.</param>
        public void AddPost(Post post)
        {
            db.Posts.Add(post);
            db.SaveChanges();        
        }

        /// <summary>
        /// Remove a post from the database.
        /// </summary>
        /// <param name="post">The post.</param>
        public void DeletePost(Post post)
        {
            db.Posts.Remove(post);
            db.SaveChanges();
        }

        /// <summary>
        /// Get all the tags.
        /// </summary>
        /// <returns>Returns a list of tags.</returns>
        public IEnumerable<Tag> GetTags() {
            return db.Tags.ToList();
        }

        /// <summary>
        /// Adding a post to a specific list of tags.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="tagsS">The tags.</param>
        public void AddPostToTags(Post post, string tagsS)
        {

            List<string> tags = tagsS.Split(',').ToList();

            for(int i = 0; i < tags.Count(); i++)
            {
                var tagName = tags.ElementAt(i);
                Tag temp = db.Tags.SingleOrDefault(t => t.TagName == tagName);
                temp.Posts.Add(post);
            }
            db.SaveChanges();
         
        }

        /// <summary>
        /// Update a post with its tags.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="tagsS">The tags.</param>
        public void UpdatePostWithTags(Post post, string tagsS)
        {
            List<string> tags = tagsS.Split(',').ToList();
            var originalPost = db.Posts.Include("Tags").Single(p => p.PostID == post.PostID);
            var newTags = db.Tags.Where(t => tags.Contains(t.TagName)).ToList();
            originalPost.Tags.Clear();
            foreach(var newTag in newTags)
            {
                originalPost.Tags.Add(newTag);
            }
            originalPost.PostTitle = post.PostTitle;
            originalPost.PostContent = Sanitizer.GetSafeHtmlFragment(post.PostContent);
            db.SaveChanges();
          
        }

        /// <summary>
        /// Get tags for a specific post.
        /// </summary>
        /// <param name="id">The post's ID.</param>
        /// <returns>Returns a list of tags.</returns>
        public IEnumerable<Tag> GetTagsByPostID(int? id)
        {
            var post = db.Posts.SingleOrDefault(p => p.PostID == id);
            var tags = post.Tags.ToList();
            return tags;
        }

        /// <summary>
        /// Get tags ordered by (descending) its post count.
        /// </summary>
        /// <returns>Returns a list of tags.</returns>
        public IEnumerable<Tag> GetTopTags()
        {
            var topTags = db.Tags.OrderByDescending(t => t.Posts.Count);
            return topTags;
        }

        /// <summary>
        /// Get all the posts.
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetAllPosts()
        {
            return db.Posts.ToList();
        }

        /// <summary>
        /// Get posts by tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Returns a list of posts</returns>
        public IEnumerable<Post> GetPostByTag(string tag)
        {
            var posts = db.Posts.Where(p => p.Tags.Any(t => t.TagName == tag)).ToList();
            return posts;
        }

        /// <summary>
        /// Get the latest posts. (order by its creation date)
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetLatestPosts()
        {
            return db.Posts.OrderByDescending(p => p.CreatedAt)
                            .ToList();
        }

        /// <summary>
        /// Get a post by its ID.
        /// </summary>
        /// <param name="id">The post's ID.</param>
        /// <returns>Return a post.</returns>
        public Post GetPostById(int? id)
        {
            return db.Posts.SingleOrDefault(p => p.PostID == id);
        }

        /// <summary>
        /// Get posts by the user's ID.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetPostByUser(string userId)
        {
            return db.Posts.Where(p => p.UserID == userId).ToList();
        }

        /// <summary>
        /// Get posts by the user's username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetPostByUserName(string username)
        {
            return db.Posts.Where(p => p.User.UserName == username)
                        .ToList();
        }

        /// <summary>
        /// Get posts by a search string.
        /// </summary>
        /// <param name="search">The search term.</param>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetPostBySearch(string search)
        {
            return db.Posts.Where(p => p.PostTitle.Contains(search) || p.PostContent.Contains(search))
                        .ToList();
        }

        /// <summary>
        /// Get posts by its vote count (order by descending)
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetTopPosts()
        {
            return db.Posts.OrderByDescending(p => p.CurrentVote).ToList();
        }

        /// <summary>
        /// Add vote to a post.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void AddPostVote(PostVote vote)
        {
            db.PostVotes.Add(vote);
            db.SaveChanges();
        }

        /// <summary>
        /// Update a post's vote.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void UpdatePostVote(PostVote vote)
        {
            db.Entry(vote).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Delete a post's vote.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void DeletePostVote(PostVote vote)
        {
            db.PostVotes.Remove(vote);
            db.SaveChanges();
        }

        /// <summary>
        /// Get posts that have been flagged.
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        public IEnumerable<Post> GetFlaggedPosts()
        {
            return db.Posts.Where(p => p.FlagPoint < 0).OrderBy(p => p.FlagPoint).ToList();
        }

        /// <summary>
        /// Add a flag to a post.
        /// </summary>
        /// <param name="flag">The flag.</param>
        public void AddPostFlag(PostFlag flag)
        {
            db.PostFlags.Add(flag);
            db.SaveChanges();
        }

        /// <summary>
        /// Update a post's flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        public void UpdatePostFlag(PostFlag flag) {
            db.Entry(flag).State = EntityState.Modified;
            db.SaveChanges();
        }

        /// <summary>
        /// Delete a post's flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        public void DeletePostFlag(PostFlag flag) {
            db.PostFlags.Remove(flag);
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
        /// Update a post.
        /// </summary>
        /// <param name="post">The post.</param>
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