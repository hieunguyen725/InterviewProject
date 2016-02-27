using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class CommentFlag
    {
        public int CommentFlagID { get; set; }

        public string FlaggedUserId { get; set; }

        public int CommentID { get; set; }
    }
}