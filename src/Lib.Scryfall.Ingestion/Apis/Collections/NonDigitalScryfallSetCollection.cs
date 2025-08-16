using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Filters;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Apis.Paging;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Filters out digital-only sets from the underlying collection.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class NonDigitalScryfallSetCollection : IAsyncEnumerable<IScryfallSet>
{
    private readonly IAsyncEnumerable<IScryfallSet> _source;
    private readonly IScryfallSetFilter _filter;

    public NonDigitalScryfallSetCollection(ILogger logger)
        : this(new HttpScryfallSetCollection(logger), new NonDigitalSetFilter())
    {
    }

    private NonDigitalScryfallSetCollection(IAsyncEnumerable<IScryfallSet> source, IScryfallSetFilter filter)
    {
        _source = source;
        _filter = filter;
    }

    public async IAsyncEnumerator<IScryfallSet> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
#pragma warning disable CA2007 // IAsyncEnumerable doesn't support ConfigureAwait
        await foreach (IScryfallSet set in _source.WithCancellation(cancellationToken))
        {
            if (_filter.ShouldInclude(set))
            {
                yield return set;
            }
        }
#pragma warning restore CA2007
    }
}
