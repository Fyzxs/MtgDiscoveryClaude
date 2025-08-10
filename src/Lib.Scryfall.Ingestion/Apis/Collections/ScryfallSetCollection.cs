using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Paging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Collection of all Scryfall sets.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class ScryfallSetCollection : HttpScryfallCollection<ExtScryfallSetDto, IScryfallSet>, IAsyncEnumerable<IScryfallSet>
{
#pragma warning disable CA2000 // Dispose objects before losing scope - Managed resources will be garbage collected
    public ScryfallSetCollection() : base(new HttpScryfallSetListPaging())
#pragma warning restore CA2000
    {
    }

    protected override IScryfallSet Transform(ExtScryfallSetDto item)
    {
        return new ScryfallSet(item);
    }

    public IAsyncEnumerator<IScryfallSet> GetAsyncEnumerator(System.Threading.CancellationToken cancellationToken = default)
    {
        return Items().GetAsyncEnumerator(cancellationToken);
    }
}
