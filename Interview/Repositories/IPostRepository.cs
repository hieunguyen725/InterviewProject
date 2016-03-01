using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Repositories
{
    /// <summary>
    /// Repository for Post.
    /// </summary>
    public interface IPostRepository
    {

        /// <summary>
        /// Get all the posts.
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetAllPosts();

        /// <summary>
        /// Get posts by its vote count (order by descending)
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetTopPosts();

        /// <summary>
        /// Get all the tags.
        /// </summary>
        /// <returns>Returns a list of tags.</returns>
        IEnumerable<Tag> GetTags();

        /// <summary>
        /// Get tags for a specific post.
        /// </summary>
        /// <param name="id">The post's ID.</param>
        /// <returns>Returns a list of tags.</returns>
        IEnumerable<Tag> GetTagsByPostID(int? id);

        /// <summary>
        /// Get tags ordered by (descending) its post count.
        /// </summary>
        /// <returns>Returns a list of tags.</returns>
        IEnumerable<Tag> GetTopTags();

        /// <summary>
        /// Adding a post to a specific list of tags.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="tagsS">The tags.</param>
        void AddPostToTags(Post post, string tags);

        /// <summary>
        /// Get a post by its ID.
        /// </summary>
        /// <param name="id">The post's ID.</param>
        /// <returns>Return a post.</returns>
        Post GetPostById(int? id);

        /// <summary>
        /// Get posts by the user's ID.
        /// </summary>
        /// <param name="userId">The user's ID.</param>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetPostByUser(string userId);

        /// <summary>
        /// Get posts by the user's username.
        /// </summary>
        /// <param name="username">The user's username.</param>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetPostByUserName(string username);

        /// <summary>
        /// Get posts by a search string.
        /// </summary>
        /// <param name="search">The search term.</param>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetPostBySearch(string search);

        /// <summary>
        /// Get the latest posts. (order by its creation date)
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetLatestPosts();

        /// <summary>
        /// Add vote to a post.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void AddPostVote(PostVote vote);

        /// <summary>
        /// Update a post's vote.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void UpdatePostVote(PostVote vote);

        /// <summary>
        /// Delete a post's vote.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void DeletePostVote(PostVote vote);

        /// <summary>
        /// Get posts that have been flagged.
        /// </summary>
        /// <returns>Returns a list of posts.</returns>
        IEnumerable<Post> GetFlaggedPosts();

        /// <summary>
        /// Add a flag to a post.
        /// </summary>
        /// <param name="flag">The flag.</param>
        void AddPostFlag(PostFlag flag);

        /// <summary>
        /// Update a post's flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        void UpdatePostFlag(PostFlag flag);

        /// <summary>
        /// Delete a post's flag.
        /// </summary>
        /// <param name="flag">The flag.</param>
        void DeletePostFlag(PostFlag flag);

        /// <summary>
        /// Update a post.
        /// </summary>
        /// <param name="post">The post.</param>
        void UpdatePost(Post post);

        /// <summary>
        /// Update a post with its tags.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <param name="tagsS">The tags.</param>
        void UpdatePostWithTags(Post post, string tags);

        /// <summary>
        /// Add a post to the database.
        /// </summary>
        /// <param name="post">The post.</param>
        void AddPost(Post post);

        /// <summary>
        /// Remove a post from the database.
        /// </summary>
        /// <param name="post">The post.</param>
        void DeletePost(Post post);

        /// <summary>
        /// Save changes to the database.
        /// </summary>
        void SaveChanges();
    }
}
