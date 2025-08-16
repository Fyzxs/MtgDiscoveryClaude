using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Factories;
using Lib.Scryfall.Ingestion.Models;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Paging;
internal sealed class HttpScryfallCardListPaging : HttpScryfallListPaging<ExtScryfallCardDto>
{
    public HttpScryfallCardListPaging(IScryfallSet set, ILogger logger)
        : base(new ScryfallSetSearchUriAdapter(set), new ScryfallCardDtoFactory(), logger)
    {
    }
}
