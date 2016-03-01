using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    /// <summary>
    /// UserProfile model.
    /// </summary>
    public class UserProfile
    {

        /// <summary>
        /// User ID.
        /// </summary>
        [Key, ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        /// <summary>
        /// User's username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// ApplicationUser - navigation property.
        /// </summary>
        public virtual ApplicationUser ApplicationUser { get; set; }

        /// <summary>
        /// AboutMe (user's information).
        /// </summary>
        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

        /// <summary>
        /// User's github link.
        /// </summary>
        public string GitHubLink { get; set; }

        /// <summary>
        /// User's linkedIn link.
        /// </summary>
        public string LinkedInLink { get; set; }

        /// <summary>
        /// User's twitter link.
        /// </summary>
        public string TwitterLink { get; set; }

        /// <summary>
        /// User's facebook link.
        /// </summary>
        public string FacebookLink { get; set; }

        /// <summary>
        /// User's website.
        /// </summary>
        public string WebsiteLink { get; set; }
    }
}