using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Operators;

/// <inheritdoc />
public interface ICosmosInquisitorAsync : ICosmosContainerQueryAsyncOperator;

/// <summary>
/// Provides an abstract base class for asynchronous query operations on a Cosmos container.
/// </summary>
public abstract class CosmosInquisitorAsync : ICosmosInquisitorAsync
{
    /// <summary>
    /// The underlying async query operator that this class delegates to.
    /// </summary>
    private readonly ICosmosContainerQueryAsyncOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosInquisitorAsync"/> class.
    /// </summary>
    /// <param name="source">The container async query operator to delegate operations to.</param>
    protected CosmosInquisitorAsync(ICosmosContainerQueryAsyncOperator source) => _source = source;

    /// <summary>
    /// Asynchronously queries items from the container and returns them as an async enumerable stream.
    /// </summary>
    /// <typeparam name="T">The type of the items to query.</typeparam>
    /// <param name="queryDefinition">The query definition containing the SQL query and parameters.</param>
    /// <param name="partitionKey">The partition key for the query.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>
    /// An async enumerable of operation responses containing the queried items.
    /// </returns>
    public IAsyncEnumerable<OpResponse<T>> QueryYield<T>(
        QueryDefinition queryDefinition,
        PartitionKey partitionKey,
        CancellationToken cancellationToken = default) => _source.QueryYield<T>(queryDefinition, partitionKey, cancellationToken);
}
