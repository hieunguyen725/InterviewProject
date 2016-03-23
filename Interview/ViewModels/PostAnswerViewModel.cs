using Interview.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.ViewModels
{
    /// <summary>
    /// Detail's view model for post and answers.
    /// Author - Hieu Nguyen & Long Nguyen
    /// </summary>
    public class PostAnswerViewModel
    {

        /* Post's related fields*/
        public int PostID { get; set; }

        public DateTime CreatedAt { get; set; }

        public ApplicationUser User { get; set; }

        public string PostTitle { get; set; }

        public int CurrentVote { get; set; }

        public string UpArrowColor { get; set; }

        public string DownArrowColor { get; set; }

        public string UserFlagStatus { get; set; }

        public ICollection<Comment> Comments{ get; set; }

        [AllowHtml]
        public string PostContent { get; set; }

        /* PostAnswer's related fields */
        public int CommentID { get; set; }

        [AllowHtml]
        [StringLength(5000, MinimumLength = 10)]
        [Display(Name = "Content")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string CommentContent { get; set; }


    }
}