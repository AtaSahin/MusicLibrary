using AutoMapper;
using MusicLibraryApp.Dto;
using MusicLibraryApp.Models;

namespace MusicLibraryApp.Mapping
{
    public class CustomerProfile:Profile
    {
        public CustomerProfile() {
            //Her iki class birbirlerini dönüştürecek
            CreateMap<Track, TrackDTO>();
            CreateMap<TrackDTO, Track>();



        }    
    }
}
