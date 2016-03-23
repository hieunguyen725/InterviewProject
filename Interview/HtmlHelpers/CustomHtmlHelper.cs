using System;
using System.Web.Mvc;

namespace Interview.HtmlHelpers
{
    public static class CustomHtmlHelper
    {
        public static string TimeAgo(this HtmlHelper helper, DateTime dt)
        {
            var ts = DateTime.Now - dt;
            if (ts <= TimeSpan.FromSeconds(60))
                return string.Format("{0} seconds ago", ts.Seconds);

            if (ts <= TimeSpan.FromMinutes(60))
                return ts.Minutes > 1 ? String.Format("{0} minutes ago", ts.Minutes) : "a minute ago";

            if (ts <= TimeSpan.FromHours(24))
                return ts.Hours > 1 ? String.Format("{0} hours ago", ts.Hours) : "an hour ago";

            if (ts <= TimeSpan.FromDays(30))
                return ts.Days > 1 ? String.Format("{0} days ago", ts.Days) : "yesterday";

            if (ts <= TimeSpan.FromDays(365))
                return ts.Days > 30 ? String.Format("{0} months ago", ts.Days / 30) : "a month ago";

            else
                return ts.Days > 365 ? String.Format("{0} years ago", ts.Days / 365) : "a year ago";
            
        }
    }
}