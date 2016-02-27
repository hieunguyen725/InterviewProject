using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class Tag
    {
        
        public int TagID { get; set; }

        [StringLength(20)]
        public string TagName { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}