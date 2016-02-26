using Interview.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.ViewModels
{
    public class PostAnswerViewModel
    {

        /* Post's related fields*/
        public int PostID { get; set; }

        public DateTime?  CreatedAt { get; set; }

        public ApplicationUser User { get; set; }

        public string PostTitle { get; set; }

        public ICollection<Comment> Comments{ get; set; }

        [AllowHtml]
        public string PostContent { get; set; }

        /* PostAnswer's related fields */
        public int CommentID { get; set; }

        [AllowHtml]
        [StringLength(500, MinimumLength = 35)]
        [Display(Name = "Content")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string CommentContent { get; set; }


    }
}