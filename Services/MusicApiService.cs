using MusicLibraryApp.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class MusicApiService
{
    private readonly HttpClient _httpClient;

    public MusicApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Song>> GetSongs()
    {
        var response = await _httpClient.GetFromJsonAsync<List<Song>>("/api/songs");
        return response;
    }
}
