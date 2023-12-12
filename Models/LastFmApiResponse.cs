// Models/LastFmApiResponse.cs
using System.Collections.Generic;
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
}

public class LastFmApiArtist
{
    public string Name { get; set; }
}

public class LastFmApiImage
{
    [JsonProperty("#text")]
    public string Text { get; set; }
}
