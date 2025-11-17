using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Models;
using VotingApp.Services;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VoteStorage _voteStorage;
        private const string VotedSessionKey = "HasVoted";

        public HomeController(ILogger<HomeController> logger, VoteStorage voteStorage)
        {
            _logger = logger;
            _voteStorage = voteStorage;
        }

        public IActionResult Index()
        {
            var hasVoted = HttpContext.Session.GetString(VotedSessionKey) == "true";
            
            var model = new VoteViewModel
            {
                HasVoted = hasVoted,
                VoteCounts = hasVoted ? _voteStorage.GetVoteCounts() : new Dictionary<string, int>()
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Vote(string selectedOption)
        {
            var hasVoted = HttpContext.Session.GetString(VotedSessionKey) == "true";
            
            if (!hasVoted && !string.IsNullOrEmpty(selectedOption))
            {
                _voteStorage.AddVote(selectedOption);
                HttpContext.Session.SetString(VotedSessionKey, "true");
            }
            
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
