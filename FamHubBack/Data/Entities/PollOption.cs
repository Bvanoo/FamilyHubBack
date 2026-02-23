namespace FamHubBack.Data.Entities
{
    public class PollOption
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int VoteCount { get; set; }

        public int EventPollId { get; set; }
    }
}
