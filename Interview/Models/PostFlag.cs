using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{

    /// <summary>
    /// PostFlag model.
    /// Author - Hieu Nguyen
    /// </summary>
    public class PostFlag
    {
        /// <summary>
        /// PostFlag ID.
        /// </summary>
        public int PostFlagID { get; set; }

        /// <summary>
        /// User ID who flagged.
        /// </summary>
        public string FlaggedUserId { get; set; }

        /// <summary>
        /// Post ID (foreign key).
        /// </summary>
        public int PostID { get; set; }
    }
}