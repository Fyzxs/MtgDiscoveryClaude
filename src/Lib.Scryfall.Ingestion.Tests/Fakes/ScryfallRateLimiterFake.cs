using System;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Http;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallRateLimiterFake : IScryfallRateLimiter
{
    public IRateLimitToken AcquireTokenAsyncResult { get; init; }
    public int AcquireTokenAsyncInvokeCount { get; private set; }

    public Task<IRateLimitToken> AcquireTokenAsync()
    {
        AcquireTokenAsyncInvokeCount++;
        return Task.FromResult(AcquireTokenAsyncResult ?? new RateLimitTokenFake());
    }
}

internal sealed class RateLimitTokenFake : IRateLimitToken
{
    public DateTime AcquiredAt { get; init; } = DateTime.UtcNow;
    public int DisposeInvokeCount { get; private set; }

    public void Dispose()
    {
        DisposeInvokeCount++;
    }
}
