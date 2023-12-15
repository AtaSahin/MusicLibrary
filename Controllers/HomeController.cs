
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibraryApp.Helpers;
using MusicLibraryApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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



      
        public async Task<IActionResult> Index(int? limit)
        {
            int songsLimit = limit ?? 8; // Default olarak 8 şarkı gösterilecek
            var topTracksJson = await _lastFmService.GetTopTracksAsync(songsLimit);

            if (topTracksJson != null)
            {
                var tracks = ConvertToYourModel(topTracksJson);
                return View(tracks);
            }

            return View();
        }






        private List<LastFmTrack>? ConvertToYourModel(string topTracksJson)
        {
            var tracksResponse = JsonConvert.DeserializeObject<LastFmApiResponse>(topTracksJson);

            return tracksResponse?.Tracks?.TrackList?.Select(t => new LastFmTrack
            {
                Name = t.Name,
                Artist = t.Artist?.Name,
                Url = t.Url,
                Duration = t.Duration,
                Playcount = t.Playcount,
                Listeners = t.Listeners,
                Mbid = t.Mbid,
                ImageUrlLarge = t.Image?.FirstOrDefault(i => i.Size == "large")?.Text,


            }).ToList();
        }


       


        //Playliste şarkı ekleme
        public IActionResult AddToPlaylist(Track track)
        {
            var playlist = HttpContext.Session.GetObjectFromJson<Playlist>("Playlist") ?? new Playlist();

         
            playlist.Tracks.Add(track);

            // Playlist session olarak kaydedildi
            HttpContext.Session.SetObjectAsJson("Playlist", playlist);

            //Düzenlenen playlist sayfasına yönlendirildi fakat düzeltilecek...
            return RedirectToAction("");
        }

        // Playlistten şarkı çıkarma
        public IActionResult RemoveFromPlaylist(int trackId)
        {
            var playlist = HttpContext.Session.GetObjectFromJson<Playlist>("Playlist") ?? new Playlist();

            var track = playlist.Tracks.FirstOrDefault(t => t.Id == trackId);

            if (track != null)
            {
                playlist.Tracks.Remove(track);
            }

            // Playlist session olarak kaydedildi
            HttpContext.Session.SetObjectAsJson("Playlist", playlist);

            //Düzenlenen playlist sayfasına yönlendirildi
            return RedirectToAction("ViewPlaylist");
        }

        private Track GetTrackDetails(int trackId)
        {
          

            return new Track {  };
        }



        public IActionResult ViewPlaylist()
        {
            var playlist = HttpContext.Session.GetObjectFromJson<Playlist>("Playlist") ?? new Playlist();
            return View(playlist);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
    }


}
