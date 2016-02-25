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

        [Display(Name = "Title")]
        [Required]
        public string PostTitle { get; set; }
        
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        [Required]
        public string PostContent { get; set; }

        [Display(Name = "Category")]
        public String SelectedCategory { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public int ViewCount { get; set; }

        [Display(Name = "Created At")]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }


    }
}