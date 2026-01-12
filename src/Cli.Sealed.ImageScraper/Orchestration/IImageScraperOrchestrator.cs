using System.Threading;
using System.Threading.Tasks;

namespace Cli.Sealed.ImageScraper.Orchestration;

internal interface IImageScraperOrchestrator
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
