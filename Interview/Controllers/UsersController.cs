using Interview.Models;
using Interview.Repositories;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Interview.Controllers
{
    public class UsersController : Controller
    {
        private IPostRepository _repo;
        private ICommentRepository _commentRepo;
        private IUserRepository _userRepo;

        public UsersController(IPostRepository _repo, ICommentRepository _commentRepo, IUserRepository userRepo)
        {
            this._repo = _repo;
            this._commentRepo = _commentRepo;
            _userRepo = userRepo;
        }

        public new ActionResult Profile(string username) {

            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.GetUserName();
            }
            var user = _userRepo.GetUserProfile(username);

            if (user == null)
            {
                return new HttpNotFoundResult();
            }
            ViewBag.isCurrentUser = false;
            ViewBag.username = username;
            if (user.Username == User.Identity.Name && User.Identity.IsAuthenticated) ViewBag.isCurrentUser = true;            
            return View(user);
        }

        [Authorize]
        public ActionResult UpdateProfile() {
            UserProfile up = _userRepo.GetUserProfile(User.Identity.Name);
            return View(up);           
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile([Bind(Include = "UserID,Username,AboutMe,FacebookLink,TwitterLink,WebsiteLink,LinkedInLink,GitHubLink")]
                                           UserProfile user)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                _userRepo.UpdateUserProfile(user);
                return RedirectToAction("Profile", new { username = user.Username });
            } 
            return View(user);
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
            if (string.IsNullOrEmpty(username))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comments = _commentRepo.GetCommentsByUser(username);
            List<Post> posts = new List<Post>();
            comments.ToList().ForEach(c => posts.Add(c.Post));
            ViewBag.postCount = comments.Count();
            return PartialView("_CommentedOn", posts.Distinct());
        }

        public ActionResult LatestPostsByUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.GetUserName();
            }
            var posts = _repo.GetPostByUserName(username)
                        .OrderByDescending(p=>p.CreatedAt)
                        .Take(5);
            ViewBag.postCount = posts.Count();
            ViewBag.username = username;
            return PartialView("_LatestPostsByUser", posts);
        }
    }
}