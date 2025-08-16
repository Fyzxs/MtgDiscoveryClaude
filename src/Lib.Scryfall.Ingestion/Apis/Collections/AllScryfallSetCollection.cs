using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Lib.Scryfall.Ingestion.Apis.Models;
using Microsoft.Extensions.Logging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Provides access to all Scryfall sets without filtering.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class AllScryfallSetCollection : IAsyncEnumerable<IScryfallSet>
{
    private readonly IAsyncEnumerable<IScryfallSet> _source;

    public AllScryfallSetCollection(ILogger logger)
        : this(new HttpScryfallSetCollection(logger))
    {
    }

    private AllScryfallSetCollection(IAsyncEnumerable<IScryfallSet> source)
    {
        _source = source;
    }

    public IAsyncEnumerator<IScryfallSet> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return _source.GetAsyncEnumerator(cancellationToken);
    }
}