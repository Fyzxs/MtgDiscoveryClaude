using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Apis.Dtos;
using Lib.Scryfall.Ingestion.Apis.Paging;

namespace Lib.Scryfall.Ingestion.Apis.Collections;

/// <summary>
/// Base implementation for Scryfall API data collections.
/// </summary>
[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
public abstract class HttpScryfallCollection<TDto, TDomain> : IHttpScryfallCollection<TDomain>
    where TDto : IScryfallDto
{
    private readonly IScryfallListPaging<TDto> _paging;
    private readonly IScryfallDtoTransformer<TDto, TDomain> _transformer;

    protected HttpScryfallCollection(IScryfallListPaging<TDto> paging, IScryfallDtoTransformer<TDto, TDomain> transformer)
    {
        _paging = paging;
        _transformer = transformer;
    }

    public async IAsyncEnumerable<TDomain> Items()
    {
        await foreach (TDto dto in _paging.Items().ConfigureAwait(false))
        {
            yield return _transformer.Transform(dto);
        }
    }

    public IAsyncEnumerator<TDomain> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return Items().GetAsyncEnumerator(cancellationToken);
    }
}
