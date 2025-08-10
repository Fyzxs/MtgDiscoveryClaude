using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Paging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Collection for Scryfall API data.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public sealed class HttpScryfallCollection<TDto, TDomain>
    where TDto : IScryfallDto
{
    private readonly IScryfallListPaging<TDto> _paging;
    private readonly IScryfallDtoTransformer<TDto, TDomain> _transformer;

    public HttpScryfallCollection(IScryfallListPaging<TDto> paging, IScryfallDtoTransformer<TDto, TDomain> transformer)
    {
        _paging = paging;
        _transformer = transformer;
    }

    public async IAsyncEnumerable<TDomain> Items()
    {
#pragma warning disable CA2007 // IAsyncEnumerable doesn't support ConfigureAwait
        await foreach (TDto dto in _paging.Items())
        {
            yield return _transformer.Transform(dto);
        }
#pragma warning restore CA2007
    }
}
