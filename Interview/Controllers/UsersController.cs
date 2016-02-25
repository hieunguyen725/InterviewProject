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
        public UsersController(IPostRepository _repo)
        {
            this._repo = _repo;
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
    }
}