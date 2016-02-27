using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Interview.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [AllowHtml]
        [StringLength(500, MinimumLength = 35)]
        [Display(Name = "Content")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string CommentContent { get; set; }

        [Display(Name = "Commented on")]
        public DateTime CreatedAt { get; set; }

        public int CurrentVote { get; set; }

        public string UpArrowColor { get; set; }

        public string DownArrowColor { get; set; }

        public virtual ICollection<CommentVote> VoteList { get; set; }

        public int PostID { get; set; }

        public virtual Post Post { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}