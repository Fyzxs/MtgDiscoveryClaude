using System;
using System.Threading;

namespace Lib.Scryfall.Ingestion.Apis.Http;

/// <summary>
/// Token representing permission to proceed with a rate-limited operation.
/// </summary>
internal sealed class RateLimitToken : IRateLimitToken
{
    private readonly SemaphoreSlim _semaphore;

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
        _semaphore.Release();
    }
}