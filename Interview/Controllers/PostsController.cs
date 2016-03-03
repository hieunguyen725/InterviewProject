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

    /// <summary>
    /// Controller for Posts.
    /// </summary>
    [Authorize]
    public class PostsController : Controller
    {
        /// <summary>
        /// Posts repository.
        /// </summary>
        private IPostRepository repo;

        /// <summary>
        /// Not hightlighted color for the post's vote arrow.
        /// </summary>
        private string notHightLightedColor;

        /// <summary>
        /// Hightlighted color for the post's vote arrow.
        /// </summary>
        private string hightLightedColor;

        /// <summary>
        /// PostsController's constructor.
        /// </summary>
        /// <param name="repo">The post repository.</param>
        public PostsController(IPostRepository repo)
        {
            this.repo = repo;
            notHightLightedColor = "rgb(0, 0, 0)";
            hightLightedColor = "rgb(250, 128, 114)";
        }

        /// <summary>
        /// Process The post flag/report by the user given the postid.
        /// </summary>
        /// <param name="postId">Id of the post.</param>
        /// <returns>Returns 1 for unflag and -1 for flag.</returns>
        public int ProcessPostFlag(int postId)
        {
            string userId = User.Identity.GetUserId();
            Post post = repo.GetPostById(postId);
            foreach (PostFlag flag in post.PostFlags)
            {
                if (flag.FlaggedUserId == userId) // if user already flag
                {
                    repo.DeletePostFlag(flag);
                    post.FlagPoint++;
                    repo.UpdatePost(post);
                    return 1;
                }
            }
            // user did not flag
            PostFlag postFlag = new PostFlag
            {
                FlaggedUserId = userId,
                PostID = post.PostID
            };
            repo.AddPostFlag(postFlag);
            post.FlagPoint--;
            repo.UpdatePost(post);
            return -1;
        }

        /// <summary>
        /// Process the post vote by the user given the vote status 
        /// and the post id.
        /// </summary>
        /// <param name="voteStatus">The status of the vote. Upvote or downvote.</param>
        /// <param name="postId">Id of the post.</param>
        /// <returns>The current vote points of this post.</returns>
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

        /// <summary>
        /// Get all the tags and return a json result.
        /// </summary>
        /// <returns>JSON result of the tags.</returns>
        [HttpGet]
        public JsonResult GetTags()
        {
            var tags = (List<Tag>)repo.GetTags();
            // Serializer doesn't like circular reference so I have
            // to return only what the view needs.           
            return Json(GetTagNames(tags), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the tags that are associated with this post given the post id.
        /// </summary>
        /// <param name="id">The id of the post.</param>
        /// <returns>JSON result of the tags.</returns>
        [HttpGet]
        public ActionResult GetTagsByPostID(int? id)
        {
            if (id == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            var tags = (List<Tag>)repo.GetTagsByPostID(id);
            // Serializer doesn't like circular reference so I have
            // to return only what the view needs.
            return Json(GetTagNames(tags), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the top tags and return a partial view.
        /// </summary>
        /// <returns>Partial view of the top tags.</returns>
        [AllowAnonymous]
        public ActionResult TopTags()
        {
            return PartialView("_TopTags", repo.GetTopTags());
        }

        /// <summary>
        /// Get the latest posts and return a partial view.
        /// </summary>
        /// <returns>Partial view of latest posts.</returns>
        [AllowAnonymous]
        public PartialViewResult LatestPosts()
        {
            var model = repo.GetLatestPosts();
            return PartialView("_LatestPosts", model);
        }

        /// <summary>
        /// Search for the related posts given the search query.
        /// </summary>
        /// <param name="search">The search query by the user.</param>
        /// <param name="page">The current page of the page list, default is 1.</param>
        /// <param name="size">The max number of posts on one page, default is 10.</param>
        /// <returns>Returns a view of related posts for that search.</returns>
        [AllowAnonymous]
        public ActionResult Search(string search, int page = 1, int size = 10)
        {
            if (string.IsNullOrEmpty(search)) return RedirectToAction("Index");
            var model = repo.GetPostBySearch(search);
            ViewBag.searchTerm = search;
            ViewBag.isPlural = model.Count() > 1 ? "results" : "result";
            PagedList<Post> pagedModel = new PagedList<Post>
                (model, page, size);
            return View(pagedModel);
        }

        /// <summary>
        /// Filter the posts base on all, latest, or hotest posts and
        /// return a partial view of the posts for that filter.
        /// </summary>
        /// <param name="filter">The filter being used (all, lastest, hotest).</param>
        /// <param name="page">The current page of the page list, default is 1.</param>
        /// <param name="size">The max number of posts on one page, default is 10.</param>
        /// <returns>Returns a partial view of related posts for that filter.</returns>
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
            ViewBag.userId = User.Identity.GetUserId();
            PagedList<Post> pagedModel = new PagedList<Post>
                (posts, page, size);
            return PartialView("_Posts", pagedModel);
        }

        /// <summary>
        /// This tags action returns a list of posts that have the tag
        /// that matches the tag search term.
        /// </summary>
        /// <param name="tag">The tag search query by the user.</param>
        /// <param name="page">The current page of the page list, default is 1.</param>
        /// <param name="size">The max number of posts on one page.</param>
        /// <returns>Returns a view of related posts that has the tag.</returns>
        [AllowAnonymous]
        public ActionResult Tags(string tag, int page = 1, int size = 10)
        {

            if (string.IsNullOrEmpty(tag))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var posts = repo.GetPostByTag(tag);
            ViewBag.Tag = tag;
            ViewBag.isPlural = posts.Count() > 1 ? "results" : "result";
            PagedList<Post> pagedModel = new PagedList<Post>
                (posts, page, size);

            return View(pagedModel);
        }

        /// <summary>
        /// This is the action that return the home page.
        /// </summary>
        /// <param name="page">The current page of the page list, default is 1.</param>
        /// <param name="size">The max number of posts on one page.</param>
        /// <returns>Returns a view with all posts.</returns>
        [AllowAnonymous]
        public ActionResult Index(int page = 1, int size = 10)
        {
            ViewBag.userId = User.Identity.GetUserId();
            PagedList<Post> pagedModel = new PagedList<Post>
                (repo.GetAllPosts(), page, size);
            return View(pagedModel);
        }

        /// <summary>
        /// Process the post's vote system status, flag status and its comments
        /// then return a view model of that post and the comments given the post id.
        /// </summary>
        /// <param name="id">id of the post</param>
        /// <returns>A view model of that post and its comments.</returns>
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
                // check comment flag status
                foreach (var flag in comment.CommentFlags)
                {
                    if (flag.FlaggedUserId == userId)
                    {
                        comment.UserFlagStatus = "Unflag";
                        break;
                    }
                }
                commentViews.Add(comment);
            }
            // check flag status to display
            string flagStatus = "Flag";
            foreach (var PostFlag in post.PostFlags)
            {
                if (PostFlag.FlaggedUserId == userId)
                {
                    flagStatus = "Unflag";
                    break;
                }
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
                DownArrowColor = post.DownArrowColor,
                UserFlagStatus = flagStatus
                
            };
            return View(vm);
        }

        /// <summary>
        /// Return the view to create a new post.
        /// </summary>
        /// <returns>Returns page view of post create.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Create a new post given its binded model.
        /// </summary>
        /// <param name="post">The binded post model to be created.</param>
        /// <returns>Redirect to index if success, else return the create view again.</returns>
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
                post.FlagPoint = 0;
                repo.AddPost(post);
                repo.AddPostToTags(post, Request["tags"]);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        /// <summary>
        /// Get the post model by its id and return an edit view.
        /// </summary>
        /// <param name="id">id of the post</param>
        /// <returns>Returns an edit view with the retrieved post.</returns>
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            Post post = repo.GetPostById(id);
            if(User.Identity.GetUserId() != post.UserID)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }
        /// <summary>
        /// Process the edited post given its binded model.
        /// </summary>
        /// <param name="post">Binded model of the edited post.</param>
        /// <returns>
        /// Redirect to the index page if the binded model is valid and updated,
        /// else return the edit view with the post again.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostID,PostTitle,PostContent,CreatedAt,UserID,ViewCount,"
            + "CurrentVote,UpArrowColor,DownArrowColor,FlagPoint")] Post post)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(Request["tags"]))
            {
                repo.UpdatePostWithTags(post, Request["tags"]);
                return RedirectToAction("Index");
            }
            return View(post);
        }

        /// <summary>
        /// Get the post model by its id and return a deletion view.
        /// </summary>
        /// <param name="id">id of the post</param>
        /// <returns>Returns an delete view with the retrieved post.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
            Post post = repo.GetPostById(id);
            if (User.Identity.GetUserId() != post.UserID)
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        /// <summary>
        /// Get the post model by its id and delete that post.
        /// </summary>
        /// <param name="id">id of the post</param>
        /// <returns>Redirect to the index page.</returns>
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

        /// <summary>
        /// Get and return a list of of string tag names given a
        /// list of tags model.
        /// </summary>
        /// <param name="tags">List of tags model.</param>
        /// <returns>Returns the list of tag names.</returns>
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
