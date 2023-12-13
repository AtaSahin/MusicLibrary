namespace MusicLibraryApp.Models
{
    public class PlaylistModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LastFmTrack> Tracks { get; set; }
    }

}
