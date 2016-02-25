using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<Post> Posts { get; set; }
    }
}