using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Dtos;
using Lib.Scryfall.Ingestion.Internal.Factories;
using Lib.Scryfall.Ingestion.Internal.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Paging;
internal sealed class HttpScryfallCardListPaging : HttpScryfallListPaging<ExtScryfallCardDto>
{
    public HttpScryfallCardListPaging(IScryfallSet set, ILogger logger)
        : base(new ScryfallSetSearchUriAdapter(set), new ScryfallCardDtoFactory(), logger)
    {
    }
}
