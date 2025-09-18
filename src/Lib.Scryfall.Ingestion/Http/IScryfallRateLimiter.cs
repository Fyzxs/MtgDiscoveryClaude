using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Http;

internal interface IScryfallRateLimiter
{
    Task<IRateLimitToken> AcquireTokenAsync();
}
