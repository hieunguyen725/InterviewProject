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

    /// <summary>
    /// Controller for comments.
    /// Authors - Hieu Nguyen & Long Nguyen
    /// </summary>
    [Authorize]
    public class CommentsController : Controller
    {
        /// <summary>
        /// Comment repository.
        /// </summary>
        private ICommentRepository repo;

        /// <summary>
        /// Not highlighted color of comment vote arrows.
        /// </summary>
        private string notHightLightedColor;


        /// <summary>
        /// CommentControllor's constructor.
        /// </summary>
        /// <param name="repo">The comment repository.</param>
        public CommentsController(ICommentRepository repo)
        {
            this.repo = repo;
            notHightLightedColor = "rgb(0, 0, 0)";
        }

        /// <summary>
        /// Process The comment flag/report by the user given the commentId.
        /// </summary>
        /// <param name="commentId">Id of the comment.</param>
        /// <returns>Returns 1 for unflag and -1 for flag.</returns>
        public int ProcessCommentFlag(int commentId)
        {
            string userId = User.Identity.GetUserId();
            return repo.ProcessCommentFlag(commentId, userId);
        }

        /// <summary>
        /// Process the comment vote by the user given the vote status 
        /// and the comment id.
        /// </summary>
        /// <param name="voteStatus">The status of the vote. Upvote or downvote.</param>
        /// <param name="commentId">Id of the comment.</param>
        /// <returns>The current vote points of this comment.</returns>
        public string ProcessCommentVote(int voteStatus, int commentId)
        {
            string userId = User.Identity.GetUserId();
            return "&nbsp;" + repo.ProcessCommentVote(voteStatus, commentId, userId);
        }

        /// <summary>
        /// Add new comment with the binded view model.
        /// </summary>
        /// <param name="vm">View model of post comment, binded with PostID and comment content.</param>
        /// <returns>Redirect action to the details view of Posts.</returns>
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
                    PostID = vm.PostID,
                    CurrentVote = 0,
                    UpArrowColor = notHightLightedColor,
                    DownArrowColor = notHightLightedColor,
                    UserFlagStatus = "Flag",
                    FlagPoint = 0
            };
                repo.AddComment(comment);
                return RedirectToAction("Details", "Posts", new { id = vm.PostID });
            }
            return RedirectToAction("Details", "Posts", new { id = vm.PostID });
        }

        /// <summary>
        /// GET method to edit a comment given its id.
        /// </summary>
        /// <param name="vm">View model of post comment, binded with PostID and comment content.</param>
        /// <returns>The view to edit the comment.</returns>
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
            if (User.Identity.GetUserId() != comment.UserID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(comment);
        }

        /// <summary>
        /// Edit a comment and update it with the binded comment.
        /// </summary>
        /// <param name="comment">The comment to be edited and update.</param>
        /// <returns>Details view of post if success, else return edit comment view.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,CommentContent,CreatedAt,PostID,UserID"
            + ",CurrentVote,UpArrowColor,DownArrowColor,UserFlagStatus,FlagPoint")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                repo.UpdateComment(comment);
                return RedirectToAction("Details", "Posts", new { id= comment.PostID});
            }
            return View(comment);
        }

        /// <summary>
        /// Get the comment model given its id and return the associate view for delete.
        /// </summary>
        /// <param name="id">Id of the comment.</param>
        /// <returns>Return the view of the comment the be delete.</returns>
        public ActionResult Delete(int? id)
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
            if (User.Identity.GetUserId() != comment.UserID && !User.IsInRole("Admin"))
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(comment);
        }

        /// <summary>
        /// Delete the selected comment given its id.
        /// </summary>
        /// <param name="id">Id of the comment.</param>
        /// <returns>Redirect action to details view of the post associated with previous comment.</returns>
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
