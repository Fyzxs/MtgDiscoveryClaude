using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Apis.Http;

/// <summary>
/// Rate limiter for Scryfall API calls.
/// </summary>
public interface IScryfallRateLimiter
{
    /// <summary>
    /// Acquires a token to proceed with a rate-limited operation.
    /// </summary>
    Task<IRateLimitToken> AcquireTokenAsync();
}
