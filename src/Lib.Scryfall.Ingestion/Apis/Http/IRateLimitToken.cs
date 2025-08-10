using System;

namespace Lib.Scryfall.Ingestion.Apis.Http;

/// <summary>
/// Token representing permission to proceed with a rate-limited operation.
/// </summary>
public interface IRateLimitToken : IDisposable
{
    /// <summary>
    /// Gets the time when this token was acquired.
    /// </summary>
    DateTime AcquiredAt { get; }
}