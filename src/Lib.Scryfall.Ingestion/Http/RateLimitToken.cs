using System;
using System.Threading;

namespace Lib.Scryfall.Ingestion.Http;

internal sealed class RateLimitToken : IRateLimitToken
{
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed;
    public DateTime AcquiredAt { get; }

    public RateLimitToken(SemaphoreSlim semaphore, DateTime acquiredAt)
    {
        _semaphore = semaphore;
        AcquiredAt = acquiredAt;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _semaphore.Release();
        _disposed = true;
    }
}
