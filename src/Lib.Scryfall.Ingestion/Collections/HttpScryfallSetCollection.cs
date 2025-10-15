using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Paging;
using Lib.Scryfall.Ingestion.Transformers;
using Lib.Scryfall.Shared.Apis.Models;

namespace Lib.Scryfall.Ingestion.Collections;

[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
internal sealed class HttpScryfallSetCollection : HttpScryfallCollection<ExtScryfallSetDto, IScryfallSet>
{
    public HttpScryfallSetCollection(ILogger logger)
        : base(new HttpScryfallSetListPaging(logger), new ScryfallSetDtoTransformer(logger))
    {
    }
}
