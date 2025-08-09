using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <summary>
/// Defines operations for reading items from a Cosmos DB container.
/// </summary>
public interface ICosmosContainerReadOperator
{
    /// <summary>
    /// Asynchronously reads an item from the container.
    /// </summary>
    /// <typeparam name="T">The type of the domain object to read.</typeparam>
    /// <param name="item">The item identifier containing the ID and partition key.</param>
    /// <returns>A task that represents the asynchronous read operation. The task result contains the operation response with the read item.</returns>
    Task<OpResponse<T>> ReadAsync<T>(ReadPointItem item);
}