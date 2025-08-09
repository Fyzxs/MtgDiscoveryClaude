using System.Threading.Tasks;

namespace Lib.Cosmos.Apis.Operators;

/// <inheritdoc />
public interface ICosmosScribe : ICosmosContainerUpsertOperator;

/// <summary>
/// Provides an abstract base class for upsert operations on a Cosmos container.
/// </summary>
public abstract class CosmosScribe : ICosmosScribe
{
    /// <summary>
    /// The underlying upsert operator that this class delegates to.
    /// </summary>
    private readonly ICosmosContainerUpsertOperator _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosScribe"/> class.
    /// </summary>
    /// <param name="source">The container upsert operator to delegate operations to.</param>
    protected CosmosScribe(ICosmosContainerUpsertOperator source) => _source = source;

    /// <summary>
    /// Asynchronously upserts an item in the container.
    /// </summary>
    /// <typeparam name="T">The type of the item to upsert.</typeparam>
    /// <param name="item">The instance of the item to be upserted.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// the response from the upsert operation with the upserted item.
    /// </returns>
    public async Task<OpResponse<T>> UpsertAsync<T>(T item) => await _source.UpsertAsync(item).ConfigureAwait(false);
}
