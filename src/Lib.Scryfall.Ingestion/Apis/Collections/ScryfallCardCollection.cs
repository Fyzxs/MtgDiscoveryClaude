using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Paging;
using Lib.Scryfall.Ingestion.Apis.Values;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Collection of cards for a specific set.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class ScryfallCardCollection : IAsyncEnumerable<IScryfallCard>
{
    private readonly HttpScryfallCollection<ExtScryfallCardDto, IScryfallCard> _collection;
    private readonly IScryfallSet _set;

#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
    public ScryfallCardCollection(IScryfallSet set, Url url) : this(set, new HttpScryfallCardListPaging(url))
#pragma warning restore CA2000
    {
    }

    internal ScryfallCardCollection(IScryfallSet set, IScryfallListPaging<ExtScryfallCardDto> paging)
    {
        _set = set;
#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
        _collection = new HttpScryfallCollection<ExtScryfallCardDto, IScryfallCard>(paging, new ScryfallCardDtoTransformer());
#pragma warning restore CA2000
    }

    public IAsyncEnumerator<IScryfallCard> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default)
    {
        _ = cancellationToken;
#pragma warning disable CA2016 // Not forwarding cancellation token
        return _collection.Items().GetAsyncEnumerator();
#pragma warning restore CA2016
    }
}
