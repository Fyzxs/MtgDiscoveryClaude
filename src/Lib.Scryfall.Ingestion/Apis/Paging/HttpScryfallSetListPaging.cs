using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Paging;

/// <summary>
/// Paging implementation for Scryfall sets.
/// </summary>
public sealed class HttpScryfallSetListPaging : HttpScryfallListPaging<ExtScryfallSetDto>
{
    public HttpScryfallSetListPaging() : base(new Url("https://api.scryfall.com/sets"))
    {
    }

    protected override ExtScryfallSetDto CreateDto(dynamic item)
    {
        return new ExtScryfallSetDto(item);
    }
}
