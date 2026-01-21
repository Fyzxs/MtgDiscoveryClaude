using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Cli.Sealed.ImageScraper.Models;

namespace Cli.Sealed.ImageScraper.Downloading;

internal sealed class ImageDownloader : IImageDownloader
{
    private const int ThrottleDelayMs = 10;

    private readonly HttpClient _httpClient;

    public ImageDownloader(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<DownloadResult> DownloadAsync(
        string imageUrl,
        string outputPath,
        CancellationToken cancellationToken)
    {
        if (File.Exists(outputPath))
        {
            return DownloadResult.Skipped("File already exists");
        }

        try
        {
            string directory = Path.GetDirectoryName(outputPath);
            if (string.IsNullOrEmpty(directory) is false && Directory.Exists(directory) is false)
            {
                _ = Directory.CreateDirectory(directory);
            }

            using HttpResponseMessage response = await _httpClient
                .GetAsync(new Uri(imageUrl), HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            if (response.IsSuccessStatusCode is false)
            {
                return DownloadResult.Failed($"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
            }

            Stream contentStream = await response.Content
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);
            await using (contentStream.ConfigureAwait(false))
            {
                FileStream fileStream = new(outputPath, FileMode.Create, FileAccess.Write);
                await using (fileStream.ConfigureAwait(false))
                {
                    await contentStream.CopyToAsync(fileStream, cancellationToken).ConfigureAwait(false);
                }
            }

            await Task.Delay(ThrottleDelayMs, cancellationToken).ConfigureAwait(false);

            return DownloadResult.Success(outputPath);
        }
        catch (HttpRequestException ex)
        {
            return DownloadResult.Failed($"Network error: {ex.Message}");
        }
        catch (IOException ex)
        {
            return DownloadResult.Failed($"IO error: {ex.Message}");
        }
    }
}
