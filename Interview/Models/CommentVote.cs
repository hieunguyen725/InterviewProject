namespace Interview.Models
{
    /// <summary>
    /// CommentVote model.
    /// Author - Hieu Nguyen
    /// </summary>
    public class CommentVote
    {
        /// <summary>
        /// CommentVote ID.
        /// </summary>
        public int CommentVoteID { get; set; }

        /// <summary>
        /// User' ID who voted.
        /// </summary>
        public string VoteUserId { get; set; }

        /// <summary>
        /// Vote's status.
        /// </summary>
        public int VoteStatus { get; set; } // either 1 or -1

        /// <summary>
        /// Comment ID (foreign key).
        /// </summary>
        public int CommentID { get; set; }

        /// <summary>
        /// Comment - navigation property.
        /// </summary>
        public virtual Comment Comment { get; set; }
    }
}