using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Lib.Scryfall.Ingestion.Dtos;
using Lib.Scryfall.Ingestion.Paging;

namespace Lib.Scryfall.Ingestion.Tests.Fakes;

internal sealed class ScryfallListPagingFake<T> : IScryfallListPaging<T> where T : IScryfallDto
{
    public List<T> ItemsResult { get; init; } = [];
    public int ItemsInvokeCount { get; private set; }

    public async IAsyncEnumerable<T> Items([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ItemsInvokeCount++;
        foreach (T item in ItemsResult)
        {
            await Task.CompletedTask.ConfigureAwait(false);
            yield return item;
        }
    }
}
