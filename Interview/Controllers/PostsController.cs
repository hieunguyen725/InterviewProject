using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interview.Models;
using Microsoft.AspNet.Identity;
using Interview.ViewModels;
using Interview.Repositories;
using Microsoft.Security.Application;
using PagedList;

namespace Interview.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private IPostRepository repo;
        private List<string> categories;

        public PostsController(IPostRepository repo)
        {
            this.repo = repo;
            categories = new List<string>
            {
                "Data Structure", "Algorithm", "Operating System",
                "Programming Fundamentals", "Mobile Development",
                "Web Development", "Database", "Other"
            };
            ViewBag.Categories = categories;
        }

        [AllowAnonymous]
        public PartialViewResult LoadCategory(string category, int page = 1, int size = 10)
        {
            ViewBag.userId = User.Identity.GetUserId();
            ViewBag.currentCategory = category;
            PagedList<Post> pagedModel = new PagedList<Post>
                (repo.GetPostByCategory(category), page, size);
            return PartialView("_Posts", pagedModel);
        }

        [AllowAnonymous]
        public PartialViewResult LatestPosts()
        {
            var model = repo.GetLatestPosts();
            return PartialView("_LatestPosts", model);
        }

        [AllowAnonymous]
        public ActionResult Search(string search, int page = 1, int size = 10)
        {
            IEnumerable<Post> model;
            if(string.IsNullOrEmpty(search))
            {
                model = repo.GetAllPosts();
            } else
            {
                model = repo.GetPostBySearch(search);
            }
            PagedList<Post> pagedModel = new PagedList<Post>
                (model, page, size);
            return PartialView("_Posts", pagedModel);
        }

        [AllowAnonymous]
        // GET: Question
        public ActionResult Index(string currentCategory = "All", int page = 1, int size = 10)
        {
            ViewBag.userId = User.Identity.GetUserId();
            ViewBag.currentCategory = currentCategory;
            PagedList<Post> pagedModel = new PagedList<Post>
                (repo.GetPostByCategory(currentCategory), page, size);
            return View(pagedModel);
        }

        [AllowAnonymous]
        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            ViewBag.userId = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = repo.GetPostById(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            post.ViewCount++;
            repo.SaveChanges();
            PostAnswerViewModel vm = new PostAnswerViewModel
            {
                PostID = post.PostID,
                PostTitle = post.PostTitle,
                PostContent = post.PostContent,
                Comments = post.Comments,
                CreatedAt = post.CreatedAt,
                User = post.User
            };
            return View(vm);
        }

        // GET: Posts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostTitle,PostContent,SelectedCategory")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.UserID = User.Identity.GetUserId();
                post.CreatedAt = DateTime.Now;
                post.PostContent = Sanitizer.GetSafeHtmlFragment(post.PostContent);
                repo.AddPost(post);
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = repo.GetPostById(id);
            if(User.Identity.GetUserId() != post.UserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,PostTitle,PostContent,CreatedAt,UserID,ViewCount,SelectedCategory")] Post post)
        {
            if (ModelState.IsValid)
            {
                repo.UpdatePost(post);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = repo.GetPostById(id);
            if (User.Identity.GetUserId() != post.UserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = repo.GetPostById(id);
            if (post!=null)
            {
                repo.DeletePost(post);
            }
            return RedirectToAction("Index");
        }

    }
}
