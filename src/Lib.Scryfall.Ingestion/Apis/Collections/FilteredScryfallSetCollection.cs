using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Models;
using Lib.Scryfall.Ingestion.Internal.Collections;
using Lib.Scryfall.Ingestion.Internal.Configurations;
using Lib.Scryfall.Ingestion.Internal.Filters;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Applies filters to a Scryfall set collection.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class FilteredScryfallSetCollection : IAsyncEnumerable<IScryfallSet>
{
    private readonly IAsyncEnumerable<IScryfallSet> _source;
    private readonly IReadOnlyList<IScryfallSetFilter> _filters;

    public FilteredScryfallSetCollection(ILogger logger)
        : this(
            new AllScryfallSetCollection(logger),
            new ConfigScryfallIngestionConfiguration())
    {
    }

    private FilteredScryfallSetCollection(
        IAsyncEnumerable<IScryfallSet> source,
        IScryfallIngestionConfiguration ingestionConfiguration)
        : this(
            source,
            [
                new NonDigitalSetFilter(),
                new SpecificSetsFilter(ingestionConfiguration),
                new MaxSetsFilter(ingestionConfiguration)
            ])
    {
    }

    private FilteredScryfallSetCollection(IAsyncEnumerable<IScryfallSet> source, IReadOnlyList<IScryfallSetFilter> filters)
    {
        _source = source;
        _filters = filters;
    }

    public async IAsyncEnumerator<IScryfallSet> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        await foreach (IScryfallSet set in _source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (ShouldNotInclude(set)) continue;

            yield return set;
        }
    }

    private bool ShouldNotInclude(IScryfallSet set)
    {
        return _filters.Any(filter => filter.ShouldNotInclude(set));
    }
}
