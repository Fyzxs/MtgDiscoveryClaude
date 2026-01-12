using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cli.Sealed.ImageScraper.MtgJson;

internal sealed class MtgJsonCache : IMtgJsonCache
{
    private const string CacheDirectory = ".cache";
    private const string CacheFileName = "AllPrintings.json";

    private readonly string _cachePath;

    public MtgJsonCache() => _cachePath = Path.Combine(CacheDirectory, CacheFileName);

    public bool Exists() => File.Exists(_cachePath);

    public string GetCachePath() => _cachePath;

    public async Task SaveAsync(Stream contentStream, CancellationToken cancellationToken)
    {
        if (Directory.Exists(CacheDirectory) is false)
        {
            _ = Directory.CreateDirectory(CacheDirectory);
        }

        using ZipArchive archive = new(contentStream, ZipArchiveMode.Read);
        ZipArchiveEntry entry = archive.Entries.FirstOrDefault(e =>
            e.Name.Equals(CacheFileName, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Could not find {CacheFileName} in downloaded archive");

#pragma warning disable CA1849 // ZipArchiveEntry.Open() has no async alternative
        Stream entryStream = entry.Open();
#pragma warning restore CA1849
        await using (entryStream.ConfigureAwait(false))
        {
            FileStream fileStream = new(_cachePath, FileMode.Create, FileAccess.Write);
            await using (fileStream.ConfigureAwait(false))
            {
                await entryStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
