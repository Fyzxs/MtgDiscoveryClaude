using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Lib.Universal.Http;

namespace Lib.Scryfall.Ingestion.Apis.Http;

/// <summary>
/// HTTP client with rate limiting for Scryfall API.
/// </summary>
public sealed class RateLimitedHttpClient : IHttpClient
{
    private readonly IHttpClient _innerClient;
    private readonly IScryfallRateLimiter _rateLimiter;

#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
    public RateLimitedHttpClient() : this(new ScryfallHttpClient(), new ScryfallRateLimiter())
    {
    }
#pragma warning restore CA2000

    private RateLimitedHttpClient(IHttpClient innerClient, IScryfallRateLimiter rateLimiter)
    {
        _innerClient = innerClient;
        _rateLimiter = rateLimiter;
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        using (await _rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            return await _innerClient.SendAsync(request).ConfigureAwait(false);
        }
    }

    public async Task<T> ResponseAs<T>(Uri uri, CancellationToken token = default)
    {
        using (await _rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            return await _innerClient.ResponseAs<T>(uri, token).ConfigureAwait(false);
        }
    }

    public async Task SaveAsFileAsync(string url, string fileName)
    {
        using (await _rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            await _innerClient.SaveAsFileAsync(new Uri(url), fileName).ConfigureAwait(false);
        }
    }

    public async Task SaveAsFileAsync(Uri uri, string fileName)
    {
        using (await _rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            await _innerClient.SaveAsFileAsync(uri, fileName).ConfigureAwait(false);
        }
    }

    public async Task<Stream> StreamAsync(string url)
    {
        using (await _rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            return await _innerClient.StreamAsync(new Uri(url)).ConfigureAwait(false);
        }
    }

    public async Task<Stream> StreamAsync(Uri url)
    {
        using (await _rateLimiter.AcquireTokenAsync().ConfigureAwait(false))
        {
            return await _innerClient.StreamAsync(url).ConfigureAwait(false);
        }
    }
}
