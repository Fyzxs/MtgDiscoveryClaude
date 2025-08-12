using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Paging;

/// <summary>
/// Knowledge class for Scryfall set paging - knows how to configure paging for sets.
/// </summary>
public sealed class HttpScryfallSetListPaging : HttpScryfallListPaging<ExtScryfallSetDto>
{
    private const string HttpsApiScryfallComSets = "https://api.scryfall.com/sets";

    public HttpScryfallSetListPaging()
        : base(new Url(HttpsApiScryfallComSets), new ScryfallSetDtoFactory())
    {
    }
}
