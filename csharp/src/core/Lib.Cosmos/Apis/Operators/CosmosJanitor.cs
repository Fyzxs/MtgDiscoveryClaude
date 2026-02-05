using System.Threading;
using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <inheritdoc />
public interface ICosmosJanitor : ICosmosContainerDeleteOperator;

/// <summary>
/// Provides an abstract base class for delete operations on a Cosmos container.
/// </summary>
public abstract class CosmosJanitor : ICosmosJanitor
{
    /// <summary>
    /// The underlying delete operator that this class delegates to.
    /// </summary>
    private readonly ICosmosContainerDeleteOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosJanitor"/> class.
    /// </summary>
    /// <param name="source">The container delete operator to delegate operations to.</param>
    protected CosmosJanitor(ICosmosContainerDeleteOperator source) => _source = source;

    /// <summary>
    /// Asynchronously deletes an item in the container.
    /// </summary>
    /// <typeparam name="T">The type of the item to delete.</typeparam>
    /// <param name="item">The item instance to be deleted.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the response from the delete operation with the item details.
    /// </returns>
    public async Task<OpResponse<T>> DeleteAsync<T>(DeletePointItem item, CancellationToken cancellationToken = default) => await _source.DeleteAsync<T>(item, cancellationToken).ConfigureAwait(false);
}
