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
    /// <summary>
    /// Controller for users.
    /// Author - Long Nguyen
    /// </summary>
    public class UsersController : Controller
    {
        /// <summary>
        /// The post repository.
        /// </summary>
        private IPostRepository _repo;

        /// <summary>
        /// The comment repository.
        /// </summary>
        private ICommentRepository _commentRepo;

        /// <summary>
        /// The user repository.
        /// </summary>
        private IUserRepository _userRepo;

        /// <summary>
        /// UsersController's constructor.
        /// </summary>
        /// <param name="_repo">The post repository.</param>
        /// <param name="_commentRepo">The comment repository.</param>
        /// <param name="userRepo">The user repository.</param>
        public UsersController(IPostRepository _repo, ICommentRepository _commentRepo, IUserRepository userRepo)
        {
            this._repo = _repo;
            this._commentRepo = _commentRepo;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Retrieve the user profile given its username and return the
        /// associated profile view.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <returns>A profile view of the user.</returns>
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

        /// <summary>
        /// Get the view to update the user profile of the current logged in user.
        /// </summary>
        /// <returns>Return the view to update the user profile.</returns>
        [Authorize]
        public ActionResult UpdateProfile() {
            UserProfile up = _userRepo.GetUserProfile(User.Identity.Name);
            return View(up);           
        }

        /// <summary>
        /// Process and update the user profile given the binded UserProfile model.
        /// </summary>
        /// <param name="user">The binded UserProfile model.</param>
        /// <returns>Redirect to the profile page if success, else return the edit profile view.</returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProfile([Bind(Include = "UserID,Username,AboutMe,FacebookLink,TwitterLink,WebsiteLink,LinkedInLink,GitHubLink,IdentIcon")]
                                           UserProfile user)
        {
            if (ModelState.IsValid)
            {
                _userRepo.UpdateUserProfile(user);
                return RedirectToAction("Profile", new { username = user.Username });
            } 
            return View(user);
        }

        /// <summary>
        /// Get all posts for that user given the username.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <param name="page">Current page of the pagelist, default is 1.</param>
        /// <param name="size">Max number of posts on one page, default is 10.</param>
        /// <returns>Returns all the posts that are posted by that user.</returns>
        public ActionResult AllPosts(string username, int page = 1, int size = 10)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = User.Identity.GetUserName();
            }
            if (username != User.Identity.Name)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            ViewBag.username = username;
            ViewBag.userId = User.Identity.GetUserId();
            PagedList<Post> model = new PagedList<Post>(_repo.GetPostByUserName(username), page, size);
            return View(model);
        }

        /// <summary>
        /// Retrieve and return all the posts the user commented on given the username.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <returns>Partial view of the associated posts.</returns>
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

        /// <summary>
        /// Get the latest posts by the user given the username.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <returns>Partial view with the latest posts.</returns>
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