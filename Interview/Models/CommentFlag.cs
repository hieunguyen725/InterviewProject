using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Models
{

    /// <summary>
    /// CommentFlag model.
    /// </summary>
    public class CommentFlag
    {
        /// <summary>
        /// The comment flag ID.
        /// </summary>
        public int CommentFlagID { get; set; }

        /// <summary>
        /// The user's ID who flagged.
        /// </summary>
        public string FlaggedUserId { get; set; }

        /// <summary>
        /// The comment ID (foreign key)
        /// </summary>
        public int CommentID { get; set; }
    }
}