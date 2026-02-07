using System.Threading;
using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Defines operations for upserting items in a Cosmos DB container.
/// </summary>
public interface ICosmosContainerUpsertOperator
{
    /// <summary>
    /// Asynchronously upserts an item in the container.
    /// </summary>
    /// <typeparam name="T">The type of the domain object to upsert.</typeparam>
    /// <param name="item">The item to upsert.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous upsert operation. The task result contains the operation response with the upserted item.</returns>
    Task<OpResponse<T>> UpsertAsync<T>(T item, CancellationToken cancellationToken = default);
}
