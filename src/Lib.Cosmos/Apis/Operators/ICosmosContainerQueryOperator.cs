using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Defines operations for querying items in a Cosmos DB container.
/// </summary>
public interface ICosmosContainerQueryOperator
{
    /// <summary>
    /// Asynchronously queries items from the container.
    /// </summary>
    /// <typeparam name="T">The type of the domain objects to query.</typeparam>
    /// <param name="queryDefinition">The query definition containing the SQL query and parameters.</param>
    /// <param name="partitionKey">The partition key for the query.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous query operation. The task result contains the operation response with the queried items.</returns>
    Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(QueryDefinition queryDefinition, PartitionKey partitionKey, CancellationToken cancellationToken = default);
}
