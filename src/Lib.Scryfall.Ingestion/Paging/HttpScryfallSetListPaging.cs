using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;
using Lib.Scryfall.Ingestion.Internal.Dtos;
using Lib.Scryfall.Ingestion.Internal.Factories;
using Lib.Scryfall.Ingestion.Internal.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Paging;
internal sealed class HttpScryfallSetListPaging : HttpScryfallListPaging<ExtScryfallSetDto>
{
    private const string HttpsApiScryfallComSets = "https://api.scryfall.com/sets";

    public HttpScryfallSetListPaging(ILogger logger)
        : base(new ProvidedScryfallSearchUri(new Url(HttpsApiScryfallComSets)), new ScryfallSetDtoFactory(), logger)
    {
    }
}
