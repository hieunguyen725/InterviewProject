using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.Models
{
    public class Post
    {

        public int PostID { get; set; }

        [StringLength(100, MinimumLength = 40)]
        [Display(Name = "Title")]
        [Required]
        public string PostTitle { get; set; }
        
        [StringLength(5000, MinimumLength = 250)]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        [Required]
        public string PostContent { get; set; }       

        public ICollection<Comment> Comments { get; set; }

        public int ViewCount { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Tag> Tags { get; set; }

    }
}