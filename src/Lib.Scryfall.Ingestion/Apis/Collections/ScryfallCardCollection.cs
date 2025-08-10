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
public sealed class ScryfallCardCollection : HttpScryfallCollection<ExtScryfallCardDto, IScryfallCard>, IAsyncEnumerable<IScryfallCard>
{
    private readonly IScryfallSet _set;

#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
    public ScryfallCardCollection(IScryfallSet set, Url url) : base(new HttpScryfallCardListPaging(url))
#pragma warning restore CA2000
    {
        _set = set;
    }

    protected override IScryfallCard Transform(ExtScryfallCardDto item)
    {
        return new ScryfallCard(item);
    }

    public IAsyncEnumerator<IScryfallCard> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default)
    {
        _ = cancellationToken;
#pragma warning disable CA2016 // Not forwarding cancellation token
        return Items().GetAsyncEnumerator();
#pragma warning restore CA2016
    }
}
