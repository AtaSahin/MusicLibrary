using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MusicLibraryApp.Helpers;
using MusicLibraryApp.Models;
using MusicLibraryApp.Service;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicLibraryApp.Controllers
{
    [Authorize]
    [RateLimit]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LastFmService _lastFmService;
        private  LanguageService _localization;
        public HomeController(ILogger<HomeController> logger, LastFmService lastFmService)
        {
            _logger = logger;
            _lastFmService = lastFmService;
        }

        // Dil değiştirme
        public IActionResult ChangeLanguage(string culture)
        {
         Response.Cookies.Append(
                         CookieRequestCultureProvider.DefaultCookieName,
                                     CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                                                 new CookieOptions { 
                                                     Expires = DateTimeOffset.UtcNow.AddYears(1) // 1 yıl boyunca cookie geçerli olacak
           });
            return Redirect(Request.Headers["Referer"].ToString());
        }


        public IActionResult Index1()
        {
            ViewBag.Welcome = _localization.Getkey("Welcome").Value;
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return View();
        }
        public async Task<IActionResult> Index(int? limit)
        {
           
            int songsLimit = limit ?? 20; // Default olarak 20 şarkı gösterilecek
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
                Tags = t.Tags?.Split(',').ToList(),
                ImageUrl = t.Image?.FirstOrDefault(predicate: i => i.Size == "medium")?.Text,
                

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



        public IActionResult ViewPlaylist()
        {
            var playlist = HttpContext.Session.GetObjectFromJson<Playlist>("Playlist") ?? new Playlist();
            return View(playlist);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

    }

}