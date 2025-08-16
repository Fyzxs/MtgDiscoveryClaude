using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Factories;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Paging;

/// <summary>
/// Knowledge class for Scryfall card paging - knows how to configure paging for cards.
/// </summary>
internal sealed class HttpScryfallCardListPaging : HttpScryfallListPaging<ExtScryfallCardDto>
{
    public HttpScryfallCardListPaging(IScryfallSet set, ILogger logger)
        : base(set, new ScryfallCardDtoFactory(), logger)
    {
    }
}
