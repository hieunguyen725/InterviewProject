using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    /// <summary>
    /// Tag model.
    /// </summary>
    public class Tag
    {
        
        /// <summary>
        /// Tag ID.
        /// </summary>
        public int TagID { get; set; }

        /// <summary>
        /// Tag's name.
        /// </summary>
        [StringLength(20)]
        public string TagName { get; set; }

        /// <summary>
        /// Posts - navigation property.
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; }
    }
}