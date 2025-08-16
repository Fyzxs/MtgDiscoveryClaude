using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Paging;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Knowledge class for Scryfall set collections - knows how to configure collection for sets.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class HttpScryfallSetCollection : HttpScryfallCollection<ExtScryfallSetDto, IScryfallSet>
{
    public HttpScryfallSetCollection(ILogger logger)
        : base(new HttpScryfallSetListPaging(logger), new ScryfallSetDtoTransformer(logger))
    {
    }
}
