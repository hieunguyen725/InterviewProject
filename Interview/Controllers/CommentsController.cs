using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Interview.Models;
using Interview.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Security.Application;
using Interview.Repositories;

namespace Interview.Controllers
{
    [Authorize]
    public class CommentsController : Controller
    {
        private ICommentRepository repo;

        public CommentsController(ICommentRepository repo)
        {
            this.repo = repo;
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddComment([Bind(Include = "PostID, CommentContent")]PostAnswerViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Comment comment = new Comment()
                {
                    CommentContent = Sanitizer.GetSafeHtmlFragment(vm.CommentContent),
                    CreatedAt = DateTime.Now,
                    UserID = User.Identity.GetUserId(),
                    PostID = vm.PostID
                };
                repo.AddComment(comment);
                return RedirectToAction("Details", "Posts", new { id = vm.PostID });
            }
            return RedirectToAction("Details", "Posts", new { id = vm.PostID });
        }

        // GET: PostAnswers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = repo.GetCommentById(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: PostAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,CommentContent,CreatedAt,PostID,UserID")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                repo.UpdateComment(comment);
                return RedirectToAction("Details", "Posts", new { id= comment.PostID});
            }
            return View(comment);
        }

        // GET: PostAnswers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment postAnswer = repo.GetCommentById(id);
            if (postAnswer == null)
            {
                return HttpNotFound();
            }
            return View(postAnswer);
        }

        // POST: PostAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = repo.GetCommentById(id);
            repo.DeleteComment(comment);
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }

    }
}
