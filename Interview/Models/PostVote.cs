namespace Interview.Models
{
    public class PostVote
    {
        public int PostVoteID { get; set; }

        public string VoteUserId { get; set; }

        public int VoteStatus { get; set; } // either 1 or -1

        public int PostID { get; set; }

        public Post Post { get; set; }
    }
}