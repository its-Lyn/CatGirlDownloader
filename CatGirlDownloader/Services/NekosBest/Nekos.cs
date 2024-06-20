using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatGirlDownloader.Models;

namespace CatGirlDownloader.Services.NekosBest;

public class Nekos
{
    private readonly NekoClient _client = new NekoClient();

    public async Task<MemoryStream> GetKittyStream()
    {
        var kitty = await _client.GetGenericEndpoint<NekoResult<NekoImage>>("neko");
        return new MemoryStream(await _client.GetByteFromUrl(kitty.Results.First().Url));
    }
}