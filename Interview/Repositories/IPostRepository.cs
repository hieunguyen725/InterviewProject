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

        Post GetPostById(int? id);

        IEnumerable<Post> GetPostByUser(string userId);

        IEnumerable<Post> GetPostByUserName(string username);

        IEnumerable<Post> GetPostBySearch(string search);

        IEnumerable<Post> GetPostByCategory(string category);

        IEnumerable<Post> GetLatestPosts();

        void UpdatePost(Post post);

        void AddPost(Post post);

        void DeletePost(Post post);

        void SaveChanges();
    }
}
