﻿using MusicLibraryApp.Dto;

namespace MusicLibraryApp.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; }
        public List<Track> Tracks { get; set; } = new List<Track>();
        public List<TrackDTO> TrackDTOs { get; set; } = new List<TrackDTO>();
    }
}
