using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class PostFlag
    {
        public int PostFlagID { get; set; }

        public string FlaggedUserId { get; set; }

        public int PostID { get; set; }
    }
}