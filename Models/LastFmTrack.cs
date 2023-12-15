namespace MusicLibraryApp.Models { 
public class LastFmTrack
{
    public string Name { get; set; }
    public string Artist { get; set; }
    public int  Id { get; set; }
    public List<LastFmApiImage> Image { get; set; }
    public string Url { get; set; }  // Şarkı URL'si
    public int Duration { get; set; }  // Şarkı süresi
    public int Playcount { get; set; }  // Şarkının oynanma sayısı
    public int Listeners { get; set; }  // Şarkıyı dinleyen kişi sayısı
    public string Mbid { get; set; }
    public string Genres { get; set; }
    public List<string> Tags { get; set; }
    public int PlaylistId { get; set; }
   public string ImageUrl { get; set; }  

    }
}