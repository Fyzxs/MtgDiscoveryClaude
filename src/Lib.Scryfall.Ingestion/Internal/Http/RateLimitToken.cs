using System;
using System.Threading;
using Lib.Scryfall.Ingestion.Apis.Http;

namespace Lib.Scryfall.Ingestion.Internal.Http;

/// <summary>
/// Token representing permission to proceed with a rate-limited operation.
/// </summary>
internal sealed class RateLimitToken : IRateLimitToken
{
    private readonly SemaphoreSlim _semaphore;
    private bool _disposed;

    /// <summary>
    /// Gets the time when this token was acquired.
    /// </summary>
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
