namespace MusicLibraryApp.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string CoverImage { get; set; }
        public string AudioFile { get; set; }
        public string AddedByUserId { get; set; }
      
    }

}
