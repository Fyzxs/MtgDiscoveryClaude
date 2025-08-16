using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Dtos;
using Lib.Scryfall.Ingestion.Internal.Paging;
using Lib.Scryfall.Ingestion.Internal.Transformers;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Internal.Collections;
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
internal sealed class HttpScryfallCardCollection : HttpScryfallCollection<ExtScryfallCardDto, IScryfallCard>
{
    public HttpScryfallCardCollection(IScryfallSet set, ILogger logger)
        : base(new HttpScryfallCardListPaging(set, logger), new ScryfallCardDtoTransformer())
    { }
}
