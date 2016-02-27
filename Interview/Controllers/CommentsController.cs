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
        private string notHightLightedColor;

        public CommentsController(ICommentRepository repo)
        {
            this.repo = repo;
            notHightLightedColor = "rgb(0, 0, 0)";
        }

        public int ProcessCommentFlag(int commentId)
        {
            string userId = User.Identity.GetUserId();
            Comment comment = repo.GetCommentById(commentId);
            foreach (CommentFlag flag in comment.CommentFlags)
            {
                if (flag.FlaggedUserId == userId) 
                {
                    repo.DeleteCommentFlag(flag);
                    comment.FlagPoint++;
                    repo.UpdateComment(comment);
                    return 1;
                }
            }
            CommentFlag commentFlag = new CommentFlag
            {
                FlaggedUserId = userId,
                CommentID = comment.CommentID
            };
            repo.AddCommentFlag(commentFlag);
            comment.FlagPoint--;
            repo.UpdateComment(comment);
            return -1;
        }

        public int ProcessCommentVote(int voteStatus, int commentId)
        {
            string userId = User.Identity.GetUserId();
            Comment comment = repo.GetCommentById(commentId);
            CommentVote userOriginalVote = null;
            // check if the user already voted, and retrieve that vote if voted
            foreach (var vote in comment.VoteList)
            {
                if (vote.VoteUserId == userId)
                {
                    userOriginalVote = vote;
                    break;
                }
            }
            if (userOriginalVote == null) // user haven't voted
            {
                CommentVote newVote = new CommentVote
                {
                    VoteUserId = userId,
                    VoteStatus = voteStatus,
                    CommentID = commentId
                };
                repo.AddCommentVote(newVote);
                comment.CurrentVote += voteStatus;
            }
            else // user voted
            {
                int originalVoteStatus = userOriginalVote.VoteStatus;
                if (voteStatus == 1 && originalVoteStatus == 1) // cancel upvote
                {
                    repo.DeleteCommentVote(userOriginalVote);
                    comment.CurrentVote--;
                }
                else if (voteStatus == 1 && originalVoteStatus == -1) // switch to upvote
                {
                    userOriginalVote.VoteStatus = 1;
                    repo.UpdateCommentVote(userOriginalVote);
                    comment.CurrentVote += 2;

                }
                else if (voteStatus == -1 && originalVoteStatus == 1) // switch to downvote
                {
                    userOriginalVote.VoteStatus = -1;
                    repo.UpdateCommentVote(userOriginalVote);
                    comment.CurrentVote -= 2;
                }
                else if (voteStatus == -1 && originalVoteStatus == -1) // cancel downvote
                {
                    repo.DeleteCommentVote(userOriginalVote);
                    comment.CurrentVote++;
                }

            }
            repo.UpdateComment(comment);
            return comment.CurrentVote;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,CommentContent,CreatedAt,PostID,UserID,CurrentVote,UpArrowColor,DownArrowColor,UserFlagStatus,FlagPoint")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                repo.UpdateComment(comment);
                return RedirectToAction("Details", "Posts", new { id= comment.PostID});
            }
            return View(comment);
        }

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
