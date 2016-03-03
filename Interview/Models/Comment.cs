using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Interview.Models
{
    /// <summary>
    /// Comment model.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Comment's ID.
        /// </summary>
        public int CommentID { get; set; }

        /// <summary>
        /// Comment's content.
        /// </summary>
        [AllowHtml]
        [StringLength(500, MinimumLength = 10)]
        [Display(Name = "Content")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string CommentContent { get; set; }

        /// <summary>
        /// Comment's creation date.
        /// </summary>
        [Display(Name = "Commented on")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Current vote of the comment.
        /// </summary>
        public int CurrentVote { get; set; }

        /// <summary>
        /// Color of up arrow.
        /// </summary>
        public string UpArrowColor { get; set; }

        /// <summary>
        /// Color of down arrow.
        /// </summary>
        public string DownArrowColor { get; set; }

        /// <summary>
        /// Comment's votes.
        /// </summary>
        public virtual ICollection<CommentVote> VoteList { get; set; }

        /// <summary>
        /// Flags count (how many time it has been flagged)
        /// </summary>
        public int FlagPoint { get; set; }

        /// <summary>
        /// The user's flag status.
        /// </summary>
        public string UserFlagStatus { get; set; }

        /// <summary>
        /// Comment's flags.
        /// </summary>
        public virtual ICollection<CommentFlag> CommentFlags { get; set; }

        /// <summary>
        /// The post's ID (foreign key)
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Post - navigation properpty
        /// </summary>
        public virtual Post Post { get; set; }

        /// <summary>
        /// The user's ID (foreign key)
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// User - navigation property.
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}