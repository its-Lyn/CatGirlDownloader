using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CatGirlDownloader.Models;

public struct NekoImage
{
    [JsonPropertyName("artist_href")]
    public required string ArtistHref { get; set; }
    
    [JsonPropertyName("artist_name")]
    public required string ArtistName { get; set; }
    
    [JsonPropertyName("source_url")]
    public required string SourceUrl { get; set; }
    
    [JsonPropertyName("url")]
    public required string Url { get; set; }
}

public struct NekoResult<T>
{
    [JsonPropertyName("results")]
    public List<T> Results { get; set; }
}

public record KittyData(MemoryStream Stream, string Url) : IDisposable, IAsyncDisposable
{
    public void Dispose()
        => Stream.Dispose();

    public async ValueTask DisposeAsync()
        => await Stream.DisposeAsync();
}