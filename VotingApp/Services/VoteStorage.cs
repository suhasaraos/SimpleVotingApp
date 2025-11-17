namespace VotingApp.Services
{
    public class VoteStorage
    {
        private static readonly Dictionary<string, int> _votes = new()
        {
            { "Absolutely, yes!", 0 },
            { "Not really", 0 },
            { "Maybe, still exploring", 0 }
        };

        private static readonly object _lock = new object();

        public void AddVote(string option)
        {
            lock (_lock)
            {
                if (_votes.ContainsKey(option))
                {
                    _votes[option]++;
                }
            }
        }

        public Dictionary<string, int> GetVoteCounts()
        {
            lock (_lock)
            {
                return new Dictionary<string, int>(_votes);
            }
        }

        public List<string> GetOptions()
        {
            return _votes.Keys.ToList();
        }
    }
}
