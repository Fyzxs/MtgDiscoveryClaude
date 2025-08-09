using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <inheritdoc />
public interface ICosmosGopher : ICosmosContainerReadOperator;

/// <summary>
/// Provides an abstract base class for read operations on a Cosmos container.
/// </summary>
public class CosmosGopher : ICosmosGopher
{
    /// <summary>
    /// The underlying read operator that this class delegates to.
    /// </summary>
    private readonly ICosmosContainerReadOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosGopher"/> class.
    /// </summary>
    /// <param name="source">The container read operator to delegate operations to.</param>
    protected CosmosGopher(ICosmosContainerReadOperator source) => _source = source;

    /// <summary>
    /// Asynchronously reads an item from the container.
    /// </summary>
    /// <typeparam name="T">The type of the item to read.</typeparam>
    /// <param name="item">The item identifier containing the ID and partition key.</param>
    /// <returns>
    /// A task that represents the asynchronous read operation. The task result contains
    /// the response from the read operation with the read item.
    /// </returns>
    public async Task<OpResponse<T>> ReadAsync<T>(ReadPointItem item) => await _source.ReadAsync<T>(item).ConfigureAwait(false);
}
