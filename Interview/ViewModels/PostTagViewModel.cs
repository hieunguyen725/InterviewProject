using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.ViewModels
{
    public class PostTagViewModel
    {
        [Display(Name = "Title")]
        [Required]
        public string PostTitle { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Content")]
        [Required]
        public string PostContent { get; set; }

        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }

        [Required]
        public string tags { get; set; }
    }
}