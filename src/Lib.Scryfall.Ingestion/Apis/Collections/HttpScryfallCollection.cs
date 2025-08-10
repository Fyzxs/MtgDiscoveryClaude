using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Paging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Base collection for Scryfall API data.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public abstract class HttpScryfallCollection<TDto, TDomain>
    where TDto : IScryfallDto
{
    private readonly IScryfallListPaging<TDto> _paging;

    protected HttpScryfallCollection(IScryfallListPaging<TDto> paging)
    {
        _paging = paging;
    }

    protected async IAsyncEnumerable<TDomain> Items()
    {
#pragma warning disable CA2007 // IAsyncEnumerable doesn't support ConfigureAwait
        await foreach (TDto dto in _paging.Items())
        {
            yield return Transform(dto);
        }
#pragma warning restore CA2007
    }

    protected abstract TDomain Transform(TDto item);
}
