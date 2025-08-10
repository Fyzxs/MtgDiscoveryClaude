using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Paging;

/// <summary>
/// Paging implementation for Scryfall cards.
/// </summary>
public sealed class HttpScryfallCardListPaging : HttpScryfallListPaging<ExtScryfallCardDto>
{
    public HttpScryfallCardListPaging(Url url) : base(url)
    {
    }

    protected override ExtScryfallCardDto CreateDto(dynamic item)
    {
        return new ExtScryfallCardDto(item);
    }
}
