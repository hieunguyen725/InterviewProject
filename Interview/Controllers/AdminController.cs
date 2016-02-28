using Interview.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Index()
        {
            var users = _repo.GetAllUsers();
            return View(users);
        }
    }
}