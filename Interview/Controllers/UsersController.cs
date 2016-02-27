using Interview.Models;
using Interview.Repositories;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.Controllers
{
    public class UsersController : Controller
    {
        private IPostRepository _repo;
        private ICommentRepository _commentRepo;

        public UsersController(IPostRepository _repo, ICommentRepository _commentRepo)
        {
            this._repo = _repo;
            this._commentRepo = _commentRepo;
        }

        public new ActionResult Profile(string username) {

            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.GetUserName();
            }
            var posts = _repo.GetPostByUserName(username).Take(5);
            ViewBag.postCount = posts.Count();
            ViewBag.username = username;
            return View(posts);
        }

        public ActionResult AllPosts(string username, int page = 1, int size = 10)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.GetUserName();
            }              
            ViewBag.username = username;
            PagedList<Post> model = new PagedList<Post>(_repo.GetPostByUserName(username), page, size);
            return View(model);
        }

        public ActionResult CommentedOn(string username)
        {
            var comments = _commentRepo.GetCommentsByUser(username);
            List<Post> posts = new List<Post>();
            foreach(var c in comments)
            {
                posts.Add(c.Post);
            }
            return PartialView("_CommentedOn", posts);
        }
    }
}