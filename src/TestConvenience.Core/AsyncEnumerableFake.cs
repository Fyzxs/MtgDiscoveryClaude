using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestConvenience.Core;

public sealed class AsyncEnumerableFake<T> : IAsyncEnumerable<T>
{
#pragma warning disable CA1002
    public List<T> GetAsyncEnumeratorResult { get; init; }
#pragma warning restore CA1002

    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        await Task.Delay(0, cancellationToken).ConfigureAwait(false);
        foreach (T item in GetAsyncEnumeratorResult)
        {
            yield return item;
        }
    }
}
