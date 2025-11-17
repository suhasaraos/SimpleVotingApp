using VotingApp.Services;
using Xunit;

namespace VotingApp.Tests.Services
{
    public class VoteStorageTests
    {
        [Fact]
        public void AddVote_ValidOption_IncreasesVoteCount()
        {
            // Arrange
            var storage = new VoteStorage();
            var option = "Absolutely, yes!";
            var initialCounts = storage.GetVoteCounts();
            var initialCount = initialCounts[option];

            // Act
            storage.AddVote(option);
            var updatedCounts = storage.GetVoteCounts();

            // Assert
            Assert.Equal(initialCount + 1, updatedCounts[option]);
        }

        [Fact]
        public void AddVote_InvalidOption_DoesNotIncreaseCount()
        {
            // Arrange
            var storage = new VoteStorage();
            var invalidOption = "Invalid Option";
            var initialCounts = storage.GetVoteCounts();
            var totalInitialVotes = initialCounts.Values.Sum();

            // Act
            storage.AddVote(invalidOption);
            var updatedCounts = storage.GetVoteCounts();
            var totalUpdatedVotes = updatedCounts.Values.Sum();

            // Assert
            Assert.Equal(totalInitialVotes, totalUpdatedVotes);
        }

        [Fact]
        public void GetVoteCounts_ReturnsAllOptions()
        {
            // Arrange
            var storage = new VoteStorage();

            // Act
            var voteCounts = storage.GetVoteCounts();

            // Assert
            Assert.Equal(3, voteCounts.Count);
            Assert.Contains("Absolutely, yes!", voteCounts.Keys);
            Assert.Contains("Not really", voteCounts.Keys);
            Assert.Contains("Maybe, still exploring", voteCounts.Keys);
        }

        [Fact]
        public void GetVoteCounts_ReturnsNewDictionary()
        {
            // Arrange
            var storage = new VoteStorage();

            // Act
            var voteCounts1 = storage.GetVoteCounts();
            var voteCounts2 = storage.GetVoteCounts();

            // Assert
            Assert.NotSame(voteCounts1, voteCounts2);
        }

        [Fact]
        public void GetOptions_ReturnsAllOptionNames()
        {
            // Arrange
            var storage = new VoteStorage();

            // Act
            var options = storage.GetOptions();

            // Assert
            Assert.Equal(3, options.Count);
            Assert.Contains("Absolutely, yes!", options);
            Assert.Contains("Not really", options);
            Assert.Contains("Maybe, still exploring", options);
        }

        [Fact]
        public void AddVote_MultipleVotes_IncreasesCountCorrectly()
        {
            // Arrange
            var storage = new VoteStorage();
            var option = "Not really";
            var initialCounts = storage.GetVoteCounts();
            var initialCount = initialCounts[option];

            // Act
            storage.AddVote(option);
            storage.AddVote(option);
            storage.AddVote(option);
            var updatedCounts = storage.GetVoteCounts();

            // Assert
            Assert.Equal(initialCount + 3, updatedCounts[option]);
        }

        [Fact]
        public void AddVote_DifferentOptions_IncreasesRespectiveCounts()
        {
            // Arrange
            var storage = new VoteStorage();
            var option1 = "Absolutely, yes!";
            var option2 = "Maybe, still exploring";
            var initialCounts = storage.GetVoteCounts();
            var initialCount1 = initialCounts[option1];
            var initialCount2 = initialCounts[option2];

            // Act
            storage.AddVote(option1);
            storage.AddVote(option2);
            storage.AddVote(option1);
            var updatedCounts = storage.GetVoteCounts();

            // Assert
            Assert.Equal(initialCount1 + 2, updatedCounts[option1]);
            Assert.Equal(initialCount2 + 1, updatedCounts[option2]);
        }
    }
}
