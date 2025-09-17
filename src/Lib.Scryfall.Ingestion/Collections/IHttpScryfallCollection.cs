using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lib.Scryfall.Ingestion.Collections;

[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
internal interface IHttpScryfallCollection<out TDomain> : IAsyncEnumerable<TDomain>;
