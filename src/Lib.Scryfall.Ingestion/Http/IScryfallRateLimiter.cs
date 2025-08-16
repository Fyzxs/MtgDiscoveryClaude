using System.Threading.Tasks;

namespace Lib.Scryfall.Ingestion.Internal.Http;
internal interface IScryfallRateLimiter
{
    Task<IRateLimitToken> AcquireTokenAsync();
}
