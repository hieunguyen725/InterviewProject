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

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IAdminRepository _repo;
        public AdminController(IAdminRepository _repo)
        {
            this._repo = _repo;
        }

        // GET: Admin
        public ActionResult Index(int page = 1, int size = 10)
        {
            PagedList<ApplicationUser> pagedModel = new PagedList<ApplicationUser>
                (_repo.GetNonAdminUsers(), page, size);
            return View(pagedModel);
        }

        public ActionResult Admins(int page = 1, int size = 10)
        {
            PagedList<ApplicationUser> pagedModel = new PagedList<ApplicationUser>
                (_repo.GetAdminUsers(), page, size);
            return View(pagedModel);
        }

        public ActionResult FlaggedPosts(int page = 1, int size = 10)
        {
            PagedList<Post> pagedModel = new PagedList<Post>
            (_repo.GetFlaggedPosts(), page, size);
            return View(pagedModel);
        }

        public ActionResult AllPosts(int page = 1, int size = 10)
        {
            PagedList<Post> pagedModel = new PagedList<Post>
            (_repo.GetAllPosts(), page, size);
            return View(pagedModel);
        }


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