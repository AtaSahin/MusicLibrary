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
using AutoMapper;
using MusicLibraryApp.Dto;

namespace MusicLibraryApp.Controllers
{
    [Authorize]
    [RateLimit]
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly LastFmService _lastFmService;
        private readonly IMapper _mapper;
        private  LanguageService _localization;
        public HomeController(ILogger<HomeController> logger, LastFmService lastFmService, IMapper mapper)
        {
            _logger = logger;
            _lastFmService = lastFmService;
            _mapper = mapper;
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
            else
            {
                _logger.LogInformation("**Error occured while listing the song**");
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
          
            // Track sınıfı TrackDTO sınıfına dönüştü
            var trackDTO = _mapper.Map<TrackDTO>(track);

            var playlist = HttpContext.Session.GetObjectFromJson<Playlist>("Playlist") ?? new Playlist();

            playlist.TrackDTOs.Add(trackDTO);
     
            // Playlist session olarak kaydedildi
            HttpContext.Session.SetObjectAsJson("Playlist", playlist);

            //Düzenlenen playlist sayfasına yönlendirildi fakat düzeltilecek...
            return RedirectToAction("");
        }

     
        public IActionResult RemoveFromPlaylist(int id)
        {
            // İlgili şarkıyı playlistten çıkarmak için gerekli işlemleri gerçekleştir
            var playlist = HttpContext.Session.GetObjectFromJson<Playlist>("Playlist") ?? new Playlist();

            // Şarkıyı bul ve playlistten çıkar
            var trackToRemove = playlist.TrackDTOs.FirstOrDefault(t => t.Id == id);
            if (trackToRemove != null)
            {
                playlist.TrackDTOs.Remove(trackToRemove);

                // Playlist session olarak güncellenmiş halde kaydedildi
                HttpContext.Session.SetObjectAsJson("Playlist", playlist);

               // _logger.LogInformation($"**Removed from playlist: {trackToRemove.Name}**");
            }
            else
            {
                _logger.LogInformation("**Song id did not matched. Error occured**");
            }

            // Düzenlenen playlist sayfasına yönlendirildi
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