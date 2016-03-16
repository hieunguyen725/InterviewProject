namespace Interview.Models
{
    /// <summary>
    /// PostVote model.
    /// Author - Hieu Nguyen
    /// </summary>
    public class PostVote
    {
        /// <summary>
        /// PostVote ID.
        /// </summary>
        public int PostVoteID { get; set; }

        /// <summary>
        /// User ID that voted.
        /// </summary>
        public string VoteUserId { get; set; }

        /// <summary>
        /// Vote's status.
        /// </summary>
        public int VoteStatus { get; set; } // either 1 or -1

        /// <summary>
        /// Post ID (foreign key).
        /// </summary>
        public int PostID { get; set; }

        /// <summary>
        /// Post - navigation property.
        /// </summary>
        public virtual Post Post { get; set; }
    }
}