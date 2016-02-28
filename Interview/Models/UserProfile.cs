using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class UserProfile
    {

        [Key, ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        public string Username { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.MultilineText)]
        public string AboutMe { get; set; }

        public string GitHubLink { get; set; }

        public string LinkedInLink { get; set; }

        public string TwitterLink { get; set; }

        public string FacebookLink { get; set; }

        public string WebsiteLink { get; set; }
    }
}