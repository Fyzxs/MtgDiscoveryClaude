using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Factories;
using Lib.Scryfall.Ingestion.Models;
using Lib.Universal.Primitives;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Paging;

internal sealed class HttpScryfallSetListPaging : HttpScryfallListPaging<ExtScryfallSetDto>
{
    private const string HttpsApiScryfallComSets = "https://api.scryfall.com/sets";

    public HttpScryfallSetListPaging(ILogger logger)
        : base(new ProvidedScryfallSearchUri(new ProvidedUrl(HttpsApiScryfallComSets)), new ScryfallSetDtoFactory(), logger)
    {
    }
}
