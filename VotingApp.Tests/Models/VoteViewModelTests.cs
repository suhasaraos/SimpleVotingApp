using VotingApp.Models;
using Xunit;

namespace VotingApp.Tests.Models
{
    public class VoteViewModelTests
    {
        [Fact]
        public void VoteViewModel_DefaultConstructor_InitializesProperties()
        {
            // Act
            var model = new VoteViewModel();

            // Assert
            Assert.Null(model.SelectedOption);
            Assert.False(model.HasVoted);
            Assert.NotNull(model.VoteCounts);
            Assert.Empty(model.VoteCounts);
        }

        [Fact]
        public void VoteViewModel_CanSetSelectedOption()
        {
            // Arrange
            var model = new VoteViewModel();
            var option = "Absolutely, yes!";

            // Act
            model.SelectedOption = option;

            // Assert
            Assert.Equal(option, model.SelectedOption);
        }

        [Fact]
        public void VoteViewModel_CanSetHasVoted()
        {
            // Arrange
            var model = new VoteViewModel();

            // Act
            model.HasVoted = true;

            // Assert
            Assert.True(model.HasVoted);
        }

        [Fact]
        public void VoteViewModel_CanSetVoteCounts()
        {
            // Arrange
            var model = new VoteViewModel();
            var voteCounts = new Dictionary<string, int>
            {
                { "Absolutely, yes!", 5 },
                { "Not really", 3 },
                { "Maybe, still exploring", 7 }
            };

            // Act
            model.VoteCounts = voteCounts;

            // Assert
            Assert.Equal(3, model.VoteCounts.Count);
            Assert.Equal(5, model.VoteCounts["Absolutely, yes!"]);
            Assert.Equal(3, model.VoteCounts["Not really"]);
            Assert.Equal(7, model.VoteCounts["Maybe, still exploring"]);
        }
    }
}
