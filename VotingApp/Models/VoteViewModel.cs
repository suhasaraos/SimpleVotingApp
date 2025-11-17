namespace VotingApp.Models
{
    public class VoteViewModel
    {
        public string? SelectedOption { get; set; }
        public bool HasVoted { get; set; }
        public Dictionary<string, int> VoteCounts { get; set; } = new();
    }
}
