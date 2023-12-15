
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Threading.Tasks;



public class LastFmService
{
    private readonly HttpClient _httpClient;
    private const string LastFmApiKey = "76dc6d3b42d1fc9d0676d812559ef4b2";

    public LastFmService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

   
    
    public async Task<string> GetTopTracksAsync(int limit = 8)
    {
        string apiUrl = $"http://ws.audioscrobbler.com/2.0/?method=chart.gettoptracks&api_key={LastFmApiKey}&format=json&limit={limit}";

        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null;
    }
}
