using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestConvenience.Core;

/// <summary>
/// Provides a test or mock implementation of <see cref="IAsyncEnumerable{T}"/> that yields elements from a predefined
/// list asynchronously.
/// </summary>
/// <remarks>Use this class to simulate asynchronous enumeration in unit tests or scenarios where a controlled
/// sequence of items is required. The enumerator yields items from the <see cref="GetAsyncEnumeratorResult"/> property
/// in order.</remarks>
/// <typeparam name="T">The type of elements returned by the asynchronous enumerator.</typeparam>
public sealed class AsyncEnumerableFake<T> : IAsyncEnumerable<T>
{
#pragma warning disable CA1002
    public List<T> GetAsyncEnumeratorResult { get; init; }
#pragma warning restore CA1002

    /// <inheritdoc/>
    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new())
    {
        await Task.Delay(0, cancellationToken).ConfigureAwait(false);
        foreach (T item in GetAsyncEnumeratorResult)
        {
            yield return item;
        }
    }
}
