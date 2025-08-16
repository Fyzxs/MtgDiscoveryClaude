using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Paging;
using Lib.Scryfall.Ingestion.Transformers;
using Lib.Scryfall.Shared.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Collections;
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
internal sealed class HttpScryfallCardCollection : HttpScryfallCollection<ExtScryfallCardDto, IScryfallCard>
{
    public HttpScryfallCardCollection(IScryfallSet set, ILogger logger)
        : base(new HttpScryfallCardListPaging(set, logger), new ScryfallCardDtoTransformer(set))
    { }
}
