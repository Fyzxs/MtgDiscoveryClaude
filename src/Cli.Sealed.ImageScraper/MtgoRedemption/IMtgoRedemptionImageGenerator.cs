using System.Threading;
using System.Threading.Tasks;

namespace Cli.Sealed.ImageScraper.MtgoRedemption;

internal interface IMtgoRedemptionImageGenerator
{
    Task<bool> GenerateAsync(
        string setCode,
        string setName,
        bool isFoil,
        string outputPath,
        CancellationToken cancellationToken);
}
