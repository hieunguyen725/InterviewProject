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
        private string notHightLightedColor;
        private string hightLightedColor;

        public PostsController(IPostRepository repo)
        {
            this.repo = repo;
            notHightLightedColor = "rgb(0, 0, 0)";
            hightLightedColor = "rgb(250, 128, 114)";
        }

        public int ProcessPostVote(int voteStatus, int postId)
        {
            string userId = User.Identity.GetUserId();
            Post post = repo.GetPostById(postId);
            PostVote userOriginalVote = null;
            // check if the user already voted, and retrieve that vote if voted
            foreach (var vote in post.VoteList)
            {
                if (vote.VoteUserId == userId)
                {
                    userOriginalVote = vote;
                    break;
                }
            }
            if (userOriginalVote == null) // user haven't voted
            {
                PostVote newVote = new PostVote
                {
                    VoteUserId = userId,
                    VoteStatus = voteStatus,
                    PostID = postId
                };
                repo.AddPostVote(newVote);
                post.CurrentVote += voteStatus;
            }
            else // user voted
            {
                int originalVoteStatus = userOriginalVote.VoteStatus;
                if (voteStatus == 1 && originalVoteStatus == 1) // cancel upvote
                {
                    repo.DeletePostVote(userOriginalVote);
                    post.CurrentVote--;
                }
                else if (voteStatus == 1 && originalVoteStatus == -1) // switch to upvote
                {
                    userOriginalVote.VoteStatus = 1;
                    repo.UpdatePostVote(userOriginalVote);
                    post.CurrentVote += 2;

                }
                else if (voteStatus == -1 && originalVoteStatus == 1) // switch to downvote
                {
                    userOriginalVote.VoteStatus = -1;
                    repo.UpdatePostVote(userOriginalVote);
                    post.CurrentVote -= 2;
                }
                else if (voteStatus == -1 && originalVoteStatus == -1) // cancel downvote
                {
                    repo.DeletePostVote(userOriginalVote);
                    post.CurrentVote++;
                }

            }
            repo.UpdatePost(post);
            return post.CurrentVote;
        }

        [HttpGet]
        public JsonResult GetTags()
        {
            var tags = (List<Tag>)repo.GetTags();
            // Serializer doesn't like circular reference so I have
            // to return only what the view needs.           
            return Json(GetTagNames(tags), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetTagsByPostID(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var tags = (List<Tag>)repo.GetTagsByPostID(id);
            // Serializer doesn't like circular reference so I have
            // to return only what the view needs.
            return Json(GetTagNames(tags), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult TopTags()
        {
            return PartialView("_TopTags", repo.GetTopTags());
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
            ViewBag.query = search;
            PagedList<Post> pagedModel = new PagedList<Post>
                (model, page, size);
            return PartialView("_Posts", pagedModel);
        }

        [AllowAnonymous]
        public ActionResult Filter(string filter, int page = 1, int size = 10)
        {
            List<Post> posts = null;
            if (string.IsNullOrEmpty(filter))
            {
                posts = repo.GetAllPosts().ToList();
            }
            else if (filter.Equals("topPosts"))
            {
                posts = repo.GetTopPosts().ToList();
            }
            else
            {
                posts = repo.GetLatestPosts().ToList();
            }
            ViewBag.query = filter;
            PagedList<Post> pagedModel = new PagedList<Post>
                (posts, page, size);
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
            string userId = User.Identity.GetUserId();
            ViewBag.userId = userId;

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
            foreach (var vote in post.VoteList)
            {
                if (vote.VoteUserId == userId)
                {
                    if (vote.VoteStatus == 1)
                    {
                        post.UpArrowColor = hightLightedColor;
                        post.DownArrowColor = notHightLightedColor;
                    }
                    else
                    {
                        post.UpArrowColor = notHightLightedColor;
                        post.DownArrowColor = hightLightedColor;
                    }
                    break;
                }
            }
            ICollection<Comment> commentViews = new List<Comment>();
            foreach (var comment in post.Comments)
            {
                ICommentRepository commentRepo = new CommentRepository();
                Comment retrievedComment = commentRepo.GetCommentById(comment.CommentID);
                foreach (var vote in retrievedComment.VoteList)
                {
                    if (vote.VoteUserId == userId)
                    {
                        if (vote.VoteStatus == 1)
                        {
                            comment.UpArrowColor = hightLightedColor;
                            comment.DownArrowColor = notHightLightedColor;
                        }
                        else
                        {
                            comment.UpArrowColor = notHightLightedColor;
                            comment.DownArrowColor = hightLightedColor;
                        }
                    }
                }
                commentViews.Add(comment);
            }
            PostAnswerViewModel vm = new PostAnswerViewModel
            {
                PostID = post.PostID,
                PostTitle = post.PostTitle,
                PostContent = post.PostContent,
                Comments = commentViews,
                CreatedAt = post.CreatedAt,
                User = post.User,
                CurrentVote = post.CurrentVote,
                UpArrowColor = post.UpArrowColor,
                DownArrowColor = post.DownArrowColor
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
                post.CurrentVote = 0;
                post.UpArrowColor = notHightLightedColor;
                post.DownArrowColor = notHightLightedColor;
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
        public ActionResult Edit([Bind(Include = "PostID,PostTitle,PostContent,CreatedAt,UserID,ViewCount,CurrentVote,UpArrowColor,DownArrowColor")] Post post)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(Request["tags"]))
            {
                repo.UpdatePostWithTags(post, Request["tags"]);
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

        private List<string> GetTagNames(List<Tag> tags)
        {
            List<string> temp = new List<string>();
            foreach (var tag in tags)
            {
                temp.Add(tag.TagName);
            }
            return temp;
        }

    }
}
