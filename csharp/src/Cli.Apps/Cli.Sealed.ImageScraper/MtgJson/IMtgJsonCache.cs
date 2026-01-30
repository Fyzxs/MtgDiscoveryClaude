using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Sealed.ImageScraper.MtgJson;

internal interface IMtgJsonCache
{
    bool Exists();
    string GetCachePath();
    Task SaveAsync(Stream contentStream, CancellationToken cancellationToken);
}
