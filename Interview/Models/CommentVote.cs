namespace Interview.Models
{
    public class CommentVote
    {
        public int CommentVoteID { get; set; }

        public string VoteUserId { get; set; }

        public int VoteStatus { get; set; } // either 1 or -1

        public int CommentID { get; set; }

        public virtual Comment Comment { get; set; }
    }
}