using Interview.Models;
using Interview.Repositories;
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
    /// Controller for admin.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// Admin repository.
        /// </summary>
        private IAdminRepository _repo;

        /// <summary>
        /// Post repository.
        /// </summary>
        private IPostRepository _postRepo;

        /// <summary>
        /// Comment repository.
        /// </summary>
        private ICommentRepository _commentRepo;

        /// <summary>
        /// AdminController's constructor.
        /// </summary>
        /// <param name="_repo">The admin repository.</param>
        /// <param name="postRepo">The post repository.</param>
        public AdminController(IAdminRepository _repo, IPostRepository postRepo, ICommentRepository commentRepo)
        {
            this._repo = _repo;
            _postRepo = postRepo;
            _commentRepo = commentRepo;
        }

        /// <summary>
        /// Index action. Returns a view with a list of users.
        /// </summary>
        /// <param name="page">The page that the user is on.</param>
        /// <param name="size">The page's size (how many items on a page).</param>
        /// <returns>Returns the Index view.</returns>
        public ActionResult Index(int page = 1, int size = 10)
        {
            PagedList<ApplicationUser> pagedModel = new PagedList<ApplicationUser>
                (_repo.GetNonAdminUsers(), page, size);
            return View(pagedModel);
        }

        /// <summary>
        /// Admins action. Returns a view with a list of users who have admin role.
        /// </summary>
        /// <param name="page">The page that the user is on.</param>
        /// <param name="size">The page's size (how many items on a page).</param>
        /// <returns>Returns the Admins view.</returns>
        public ActionResult Admins(int page = 1, int size = 10)
        {
            PagedList<ApplicationUser> pagedModel = new PagedList<ApplicationUser>
                (_repo.GetAdminUsers(), page, size);
            return View("Admins",pagedModel);
        }

        /// <summary>
        /// FlaggedPosts action. Returns a view with a list of posts that have been flagged.
        /// </summary>
        /// <param name="page">The page that the user is on.</param>
        /// <param name="size">The page's size (how many items on a page).</param>
        /// <returns>Returns the FlaggedPosts view.</returns>
        public ActionResult FlaggedPosts(int page = 1, int size = 10)
        {
            PagedList<Post> pagedModel = new PagedList<Post>
            (_postRepo.GetFlaggedPosts(), page, size);
            return View(pagedModel);
        }

        public ActionResult FlaggedComments(int page = 1, int size = 10)
        {
            PagedList<Comment> pagedModel = new PagedList<Comment>
            (_commentRepo.GetFlaggedComments(), page, size);
            return View(pagedModel);
        }

        /// <summary>
        /// AllPosts action. Returns a view with a list of posts.
        /// </summary>
        /// <param name="page">The page that the user is on.</param>
        /// <param name="size">The page's size (how many items on a page).</param>
        /// <returns></returns>
        public ActionResult AllPosts(int page = 1, int size = 10)
        {
            PagedList<Post> pagedModel = new PagedList<Post>
            (_postRepo.GetAllPosts(), page, size);
            return View(pagedModel);
        }

        /// <summary>
        /// GrandAdminAccess action. Return a view with an user (who will be given the admin role).
        /// </summary>
        /// <param name="userName">The user's username.</param>
        /// <returns>Returns an user.</returns>
        public ActionResult GrantAdminAccess(string userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = _repo.GetUserByUsername(userName);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// GrandAdminAccessConfirm action. Add admin role to the user (redirect to Index action when done).
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Returns a redirect to Index action.</returns>
        [ValidateAntiForgeryToken]
        public ActionResult GrantAdminAccessConfirm(string userId)
        {
            if (userId != null)
            {
                _repo.AddRoleToUser("Admin", userId);
            }
            return RedirectToAction("Index");
        }
    }
}