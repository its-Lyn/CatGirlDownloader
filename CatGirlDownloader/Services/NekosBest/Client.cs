using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatGirlDownloader.Services.NekosBest;

public class NekoClient
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("https://nekos.best/api/v2/") };

    public async Task<T> GetGenericEndpoint<T>(string endpoint)
    {
        HttpResponseMessage response = await _client.GetAsync($"./{endpoint}");
        response.EnsureSuccessStatusCode();

        string content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content) ?? throw new NullReferenceException("Received no data");
    }

    public async Task<byte[]> GetByteFromUrl(string url)
        => await _client.GetByteArrayAsync(url);
}