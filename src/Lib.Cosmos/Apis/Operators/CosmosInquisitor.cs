using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Operators;

/// <inheritdoc />
public interface ICosmosInquisitor : ICosmosContainerQueryOperator;

/// <summary>
/// Provides an abstract base class for query operations on a Cosmos container.
/// </summary>
public abstract class CosmosInquisitor : ICosmosInquisitor
{
    /// <summary>
    /// The underlying query operator that this class delegates to.
    /// </summary>
    private readonly ICosmosContainerQueryOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosInquisitor"/> class.
    /// </summary>
    /// <param name="source">The container query operator to delegate operations to.</param>
    protected CosmosInquisitor(ICosmosContainerQueryOperator source) => _source = source;

    /// <summary>
    /// Asynchronously queries items from the container.
    /// </summary>
    /// <typeparam name="T">The type of the items to query.</typeparam>
    /// <param name="queryDefinition">The query definition containing the SQL query and parameters.</param>
    /// <param name="partitionKey">The partition key for the query.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>
    /// A task that represents the asynchronous query operation. The task result contains
    /// the response from the query operation with the queried items.
    /// </returns>
    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(
        QueryDefinition queryDefinition,
        PartitionKey partitionKey,
        CancellationToken cancellationToken = default) => await _source.QueryAsync<T>(queryDefinition, partitionKey, cancellationToken).ConfigureAwait(false);
}
