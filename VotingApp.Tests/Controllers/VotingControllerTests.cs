using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using VotingApp.Controllers;
using VotingApp.Models;
using VotingApp.Services;
using Xunit;

namespace VotingApp.Tests.Controllers
{
    public class VotingControllerTests
    {
        private readonly Mock<ILogger<VotingController>> _mockLogger;
        private readonly VoteStorage _voteStorage;
        private readonly VotingController _controller;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ISession> _mockSession;
        private readonly Dictionary<string, byte[]> _sessionStorage;

        public VotingControllerTests()
        {
            _mockLogger = new Mock<ILogger<VotingController>>();
            _voteStorage = new VoteStorage();
            _controller = new VotingController(_mockLogger.Object, _voteStorage);

            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new Mock<ISession>();
            _sessionStorage = new Dictionary<string, byte[]>();

            // Setup session to use a dictionary for storage
            _mockSession.Setup(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]?>.IsAny))
                .Returns((string key, out byte[]? value) =>
                {
                    return _sessionStorage.TryGetValue(key, out value);
                });

            _mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, value) =>
                {
                    _sessionStorage[key] = value;
                });

            _mockHttpContext.Setup(x => x.Session).Returns(_mockSession.Object);
            _mockHttpContext.Setup(x => x.TraceIdentifier).Returns("test-trace-id");
            
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        private void SetSessionValue(string key, string value)
        {
            _sessionStorage[key] = Encoding.UTF8.GetBytes(value);
        }

        [Fact]
        public void Index_UserHasNotVoted_ReturnsViewWithEmptyVoteCounts()
        {
            // Arrange - session is empty by default

            // Act
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as VoteViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.False(model.HasVoted);
            Assert.Empty(model.VoteCounts);
        }

        [Fact]
        public void Index_UserHasVoted_ReturnsViewWithVoteCounts()
        {
            // Arrange
            SetSessionValue("HasVoted", "true");
            _voteStorage.AddVote("Absolutely, yes!");

            // Act
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as VoteViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.True(model.HasVoted);
            Assert.NotEmpty(model.VoteCounts);
            Assert.Equal(3, model.VoteCounts.Count);
        }

        [Fact]
        public void Vote_ValidOptionAndNotVoted_AddsVoteAndSetsSession()
        {
            // Arrange
            var option = "Not really";
            var initialCounts = _voteStorage.GetVoteCounts();
            var initialCount = initialCounts[option];

            // Act
            var result = _controller.Vote(option) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(VotingController.Index), result.ActionName);
            
            var updatedCounts = _voteStorage.GetVoteCounts();
            Assert.Equal(initialCount + 1, updatedCounts[option]);
            
            // Verify session was set
            Assert.True(_sessionStorage.ContainsKey("HasVoted"));
        }

        [Fact]
        public void Vote_UserAlreadyVoted_DoesNotAddVote()
        {
            // Arrange
            var option = "Absolutely, yes!";
            SetSessionValue("HasVoted", "true");

            var initialCounts = _voteStorage.GetVoteCounts();
            var initialCount = initialCounts[option];

            // Act
            var result = _controller.Vote(option) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            var updatedCounts = _voteStorage.GetVoteCounts();
            Assert.Equal(initialCount, updatedCounts[option]);
        }

        [Fact]
        public void Vote_EmptyOption_DoesNotAddVote()
        {
            // Arrange
            var initialCounts = _voteStorage.GetVoteCounts();
            var totalInitialVotes = initialCounts.Values.Sum();

            // Act
            var result = _controller.Vote(string.Empty) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            var updatedCounts = _voteStorage.GetVoteCounts();
            var totalUpdatedVotes = updatedCounts.Values.Sum();
            Assert.Equal(totalInitialVotes, totalUpdatedVotes);
        }

        [Fact]
        public void Vote_NullOption_DoesNotAddVote()
        {
            // Arrange
            var initialCounts = _voteStorage.GetVoteCounts();
            var totalInitialVotes = initialCounts.Values.Sum();

            // Act
            var result = _controller.Vote(null!) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            var updatedCounts = _voteStorage.GetVoteCounts();
            var totalUpdatedVotes = updatedCounts.Values.Sum();
            Assert.Equal(totalInitialVotes, totalUpdatedVotes);
        }

        [Fact]
        public void Vote_AlwaysRedirectsToIndex()
        {
            // Arrange - session is empty by default

            // Act
            var result = _controller.Vote("Absolutely, yes!") as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(nameof(VotingController.Index), result.ActionName);
        }

        [Fact]
        public void Error_ReturnsErrorView()
        {
            // Act
            var result = _controller.Error() as ViewResult;
            var model = result?.Model as ErrorViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(model);
            Assert.NotNull(model.RequestId);
            Assert.Equal("test-trace-id", model.RequestId);
        }
    }
}
