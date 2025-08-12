using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;

namespace Lib.Scryfall.Ingestion.Apis.Paging;

/// <summary>
/// Knowledge class for Scryfall card paging - knows how to configure paging for cards.
/// </summary>
public sealed class HttpScryfallCardListPaging : HttpScryfallListPaging<ExtScryfallCardDto>
{
    public HttpScryfallCardListPaging(IScryfallSet set)
        : base(set, new ScryfallCardDtoFactory())
    {
    }
}
