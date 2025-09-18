using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Paging;

namespace Lib.Scryfall.Ingestion.Collections;

[SuppressMessage("Naming", "CA1711:Identifiers should not end in incorrect suffix", Justification = "Collection is appropriate for these types")]
internal abstract class HttpScryfallCollection<TDto, TDomain> : IHttpScryfallCollection<TDomain>
    where TDto : IScryfallDto
{
    private readonly IScryfallListPaging<TDto> _paging;
    private readonly IScryfallDtoTransformer<TDto, TDomain> _transformer;

    protected HttpScryfallCollection(IScryfallListPaging<TDto> paging, IScryfallDtoTransformer<TDto, TDomain> transformer)
    {
        _paging = paging;
        _transformer = transformer;
    }

    public async IAsyncEnumerable<TDomain> Items([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (TDto dto in _paging.Items(cancellationToken).ConfigureAwait(false))
        {
            yield return _transformer.Transform(dto);
        }
    }

    public IAsyncEnumerator<TDomain> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return Items(cancellationToken).GetAsyncEnumerator(cancellationToken);
    }
}
