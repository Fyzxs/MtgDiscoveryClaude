using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis.Http;

/// <summary>
/// Rate limiter for Scryfall API calls.
/// Scryfall asks for 50-100ms between requests.
/// </summary>
public sealed class ScryfallRateLimiter : IScryfallRateLimiter, IDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private DateTime _lastRequest = DateTime.MinValue;
    private const int DelayMilliseconds = 100;

    public async Task<IRateLimitToken> AcquireTokenAsync()
    {
        await _semaphore.WaitAsync().ConfigureAwait(false);

        await ApplyRateLimitDelay().ConfigureAwait(false);

        _lastRequest = DateTime.UtcNow;
        return new RateLimitToken(_semaphore, _lastRequest);
    }

    private async Task ApplyRateLimitDelay()
    {
        TimeSpan timeSinceLastRequest = DateTime.UtcNow - _lastRequest;
        int remainingDelay = DelayMilliseconds - (int)timeSinceLastRequest.TotalMilliseconds;

        if (remainingDelay <= 0) return;
        await Task.Delay(remainingDelay).ConfigureAwait(false);
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}
