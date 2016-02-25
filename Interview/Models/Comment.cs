using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Interview.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Display(Name = "Comment")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Required]
        public string CommentContent { get; set; }

        [Display(Name = "Commented on")]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public int PostID { get; set; }

        public virtual Post Post { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}