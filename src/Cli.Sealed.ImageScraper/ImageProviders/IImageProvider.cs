using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.ImageProviders;

internal interface IImageProvider
{
    string Name { get; }
    bool CanProvide(SealedProduct product);
    Task<IReadOnlyList<string>> GetImageUrlsAsync(SealedProduct product, ImageSize size, CancellationToken cancellationToken);
}
