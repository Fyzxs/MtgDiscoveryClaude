using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.Downloading;

internal interface IImageDownloader
{
    Task<DownloadResult> DownloadAsync(
        string imageUrl,
        string outputPath,
        CancellationToken cancellationToken);
}
