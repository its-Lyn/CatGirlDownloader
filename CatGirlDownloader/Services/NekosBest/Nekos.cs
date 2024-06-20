using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CatGirlDownloader.Models;

namespace CatGirlDownloader.Services.NekosBest;

public class Nekos
{
    private readonly NekoClient _client = new NekoClient();

    public async Task<KittyData> GetKittyStream()
    {
        NekoResult<NekoImage> kitty = await _client.GetGenericEndpoint<NekoResult<NekoImage>>("neko");
        string url = kitty.Results.First().Url;
        return new KittyData(
            new MemoryStream(await _client.GetByteFromUrl(url)),
            url
        );
    }
}