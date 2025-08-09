using System.Collections.Generic;
using System.Threading;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Apis.Operators;
/// <summary>
/// Defines operations for asynchronously streaming query results from a Cosmos DB container.
/// </summary>
public interface ICosmosContainerQueryAsyncOperator
{
    /// <summary>
    /// Asynchronously queries items from the container and returns them as an async enumerable stream.
    /// </summary>
    /// <typeparam name="T">The type of the domain objects to query.</typeparam>
    /// <param name="queryDefinition">The query definition containing the SQL query and parameters.</param>
    /// <param name="partitionKey">The partition key for the query.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>An async enumerable of operation responses containing the queried items.</returns>
    IAsyncEnumerable<OpResponse<T>> QueryYield<T>(QueryDefinition queryDefinition, PartitionKey partitionKey, CancellationToken cancellationToken = default);
}
