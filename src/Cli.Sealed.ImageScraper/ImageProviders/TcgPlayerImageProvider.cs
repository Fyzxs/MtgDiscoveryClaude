using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.ImageProviders;

internal sealed class TcgPlayerImageProvider : IImageProvider
{
    private const string BaseUrl = "https://tcgplayer-cdn.tcgplayer.com/product";

    public string Name => "TcgPlayer";

    public bool CanProvide(SealedProduct product) => product.HasTcgplayerProductId;

    public Task<IReadOnlyList<string>> GetImageUrlsAsync(SealedProduct product, ImageSize size, CancellationToken cancellationToken)
    {
        if (product.HasTcgplayerProductId is false)
        {
            return Task.FromResult<IReadOnlyList<string>>([]);
        }

        IReadOnlyList<string> urls = [$"{BaseUrl}/{product.TcgplayerProductId}_in_{size.Value}x{size.Value}.jpg"];
        return Task.FromResult(urls);
    }
}
