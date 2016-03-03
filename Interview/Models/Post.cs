using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.Models
{
    /// <summary>
    /// Post model.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Post ID.
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Post's Title.
        /// </summary>
        [StringLength(100, MinimumLength = 10)]
        [Display(Name = "Title")]
        [Required]
        public string PostTitle { get; set; }
        
        /// <summary>
        /// Post's content.
        /// </summary>
        [StringLength(5000, MinimumLength = 10)]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        [Required]
        public string PostContent { get; set; }       

        /// <summary>
        /// Post's comments - navigation property
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Post's view count.
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Post's current vote.
        /// </summary>
        public int CurrentVote { get; set; }

        /// <summary>
        /// Post's up arrow color.
        /// </summary>
        public string UpArrowColor { get; set; }

        /// <summary>
        /// Post's down arrow color.
        /// </summary>
        public string DownArrowColor { get; set; }

        /// <summary>
        /// Post's votes - navigation property.
        /// </summary>
        public virtual ICollection<PostVote> VoteList { get; set; }

        /// <summary>
        /// Post's flags.
        /// </summary>
        public int FlagPoint { get; set; }

        /// <summary>
        /// Post's flags - navigation property.
        /// </summary>
        public virtual ICollection<PostFlag> PostFlags { get; set; }

        /// <summary>
        /// Post's creation date.
        /// </summary>
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The user ID.
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// User - navigation property.
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Post's tags - navigation property.
        /// </summary>
        public virtual ICollection<Tag> Tags { get; set; }

    }
}