
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibraryApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MusicLibraryApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LastFmService _lastFmService;

        public HomeController(ILogger<HomeController> logger, LastFmService lastFmService)
        {
            _logger = logger;
            _lastFmService = lastFmService;
        }

        public async Task<IActionResult> Index()
        {
            var topTracksJson = await _lastFmService.GetTopTracksAsync();

            if (topTracksJson != null)
            {
                var tracks = ConvertToYourModel(topTracksJson);
                return View(tracks);
            }

            return View(); // API'dan veri çekilemediği durumu
        }

        private List<LastFmTrack> ConvertToYourModel(string topTracksJson)
        {
            var tracksResponse = JsonConvert.DeserializeObject<LastFmApiResponse>(topTracksJson);

            return tracksResponse?.Tracks?.TrackList?.Select(t => new LastFmTrack
            {
                Name = t.Name,
                Artist = t.Artist?.Name,
                ImageUrl = t.Image?.FirstOrDefault()?.Text
            }).ToList();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
