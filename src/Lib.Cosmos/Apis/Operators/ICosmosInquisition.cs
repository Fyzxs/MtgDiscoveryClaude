using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Defines an operator that executes read/query operations against a Cosmos DB collection
/// and returns results wrapped in an <see cref="OpResponse{T}"/> to surface status and errors.
/// </summary>
/// <remarks>
/// Implementations of this interface are expected to perform read/query operations and return
/// an <see cref="OpResponse{IEnumerable{T}}"/> that encapsulates the returned items, the HTTP
/// status code, and any exception information. Normal failure scenarios (for example, a non-success
/// HTTP status or a transient error) should be represented by a non-successful <see cref="OpResponse{T}"/>
/// rather than by throwing exceptions, so callers can inspect the response and handle errors deterministically.
/// </remarks>
public interface ICosmosInquisition
{
    /// <summary>
    /// Executes a query against Cosmos DB and returns the resulting sequence of items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The element type contained in the query results.</typeparam>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the query operation.
    /// If not provided, the operation is unbounded by caller-supplied cancellation.
    /// </param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> that, when completed, yields an <see cref="OpResponse{IEnumerable{T}}"/>.
    /// The response's <see cref="OpResponse{T}.StatusCode"/> indicates the HTTP level outcome, <see cref="OpResponse{T}.Value"/>
    /// provides the returned items when successful, and <see cref="OpResponse{T}.Exception()"/> can be used to retrieve
    /// an underlying exception if one was captured.
    /// </returns>
    /// <remarks>
    /// - Callers should check <see cref="OpResponse{T}.IsSuccessful"/> (or the status code) before using the returned value.
    /// - Implementations should honor the provided <paramref name="cancellationToken"/> and avoid throwing for common failure
    ///   scenarios; instead, populate the returned <see cref="OpResponse{T}"/> with appropriate status and exception information.
    /// - This method is intended for read/query semantics returning multiple items; for single-item reads consider a different contract.
    /// </remarks>
    Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(CancellationToken cancellationToken = default);
}
