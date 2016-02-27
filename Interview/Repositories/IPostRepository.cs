using Interview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Repositories
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetAllPosts();

        IEnumerable<Post> GetTopPosts();

        IEnumerable<Tag> GetTags();

        IEnumerable<Tag> GetTagsByPostID(int? id);

        IEnumerable<Tag> GetTopTags();

        void AddPostToTags(Post post, string tags);

        Post GetPostById(int? id);

        IEnumerable<Post> GetPostByUser(string userId);

        IEnumerable<Post> GetPostByUserName(string username);

        IEnumerable<Post> GetPostBySearch(string search);
        
        IEnumerable<Post> GetLatestPosts();

        void AddPostVote(PostVote vote);

        void UpdatePostVote(PostVote vote);

        void DeletePostVote(PostVote vote);

        void AddPostFlag(PostFlag flag);

        void UpdatePostFlag(PostFlag flag);

        void DeletePostFlag(PostFlag flag);

        void UpdatePost(Post post);

        void UpdatePostWithTags(Post post, string tags);

        void AddPost(Post post);

        void DeletePost(Post post);

        void SaveChanges();
    }
}
