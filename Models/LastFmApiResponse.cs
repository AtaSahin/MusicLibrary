// Models/LastFmApiResponse.cs
using System.Collections.Generic;
using MusicLibraryApp.Models;
using Newtonsoft.Json;

public class LastFmApiResponse
{
    [JsonProperty("tracks")]
    public LastFmApiTrackList Tracks { get; set; }

}

public class LastFmApiTrackList
{
    [JsonProperty("track")]
    public List<LastFmApiTrack> TrackList { get; set; }
}

public class LastFmApiTrack
{
    public string Name { get; set; }
    public LastFmApiArtist Artist { get; set; }
    public List<LastFmApiImage> Image { get; set; }
    public string Url { get; set; }  // Şarkı URL'si
    public int Duration { get; set; }  // Şarkı süresi
    public int Playcount { get; set; }  // Şarkının oynanma sayısı
    public int Listeners { get; set; }  // Şarkıyı dinleyen kişi sayısı
    public string Mbid { get; set; }  // Şarkının MusicBrainz kimliği
    public string Genres { get;  set; }
    public string Tags { get; set; }
}

public class LastFmApiArtist
{
    public string Name { get; set; }
}

public class LastFmApiImage
{
    [JsonProperty("#text")]
    public string Text { get; set; }

    public string Size { get; set; }
}
