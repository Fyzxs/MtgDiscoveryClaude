using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Defines operations for deleting items from a Cosmos DB container.
/// </summary>
public interface ICosmosContainerDeleteOperator
{
    /// <summary>
    /// Asynchronously deletes an item from the container.
    /// </summary>
    /// <typeparam name="T">The type of the domain object being deleted.</typeparam>
    /// <param name="item">The item identifier containing the ID and partition key.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result contains the operation response.</returns>
    Task<OpResponse<T>> DeleteAsync<T>(DeletePointItem item);
}
