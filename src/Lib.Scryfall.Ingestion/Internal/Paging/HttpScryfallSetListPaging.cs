using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Values;
using Lib.Scryfall.Ingestion.Internal.Factories;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Paging;

/// <summary>
/// Knowledge class for Scryfall set paging - knows how to configure paging for sets.
/// </summary>
internal sealed class HttpScryfallSetListPaging : HttpScryfallListPaging<ExtScryfallSetDto>
{
    private const string HttpsApiScryfallComSets = "https://api.scryfall.com/sets";

    public HttpScryfallSetListPaging(ILogger logger)
        : base(new ProvidedScryfallSearchUri(new Url(HttpsApiScryfallComSets)), new ScryfallSetDtoFactory(), logger)
    {
    }
}
