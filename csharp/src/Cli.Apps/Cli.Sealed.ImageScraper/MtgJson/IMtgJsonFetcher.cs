using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.MtgJson;

internal interface IMtgJsonFetcher
{
    Task<IReadOnlyList<SealedProduct>> GetSealedProductsAsync(
        IReadOnlyList<string> setCodes,
        CancellationToken cancellationToken);
}
