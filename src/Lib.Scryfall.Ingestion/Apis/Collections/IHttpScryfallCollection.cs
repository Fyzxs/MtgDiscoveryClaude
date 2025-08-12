using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Interface for Scryfall API data collections.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public interface IHttpScryfallCollection<out TDomain> : IAsyncEnumerable<TDomain>
{
}
