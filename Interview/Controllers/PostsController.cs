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

        public PostsController(IPostRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public JsonResult GetTags()
        {
            var tags = repo.GetTags();
            List<string> temp = new List<string>();
            foreach(var tag in tags) {
                temp.Add(tag.TagName);
            }
            return Json(temp, JsonRequestBehavior.AllowGet);
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
        public ActionResult Index(int page = 1, int size = 10)
        {
            ViewBag.userId = User.Identity.GetUserId();
            PagedList<Post> pagedModel = new PagedList<Post>
                (repo.GetAllPosts(), page, size);
            return View(pagedModel);
        }

        [AllowAnonymous]
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostTitle,PostContent")] Post post)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(Request["tags"]))
            {
                ViewBag.noTag = false;
                post.UserID = User.Identity.GetUserId();
                post.CreatedAt = DateTime.Now;
                post.PostContent = Sanitizer.GetSafeHtmlFragment(post.PostContent);
                repo.AddPost(post);
                repo.AddPostToTags(post, Request["tags"]);
                return RedirectToAction("Index");
            }
            return View(post);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,PostTitle,PostContent,CreatedAt,UserID,ViewCount")] Post post)
        {
            if (ModelState.IsValid)
            {
                repo.UpdatePost(post);
                return RedirectToAction("Index");
            }
            return View(post);
        }
        
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
