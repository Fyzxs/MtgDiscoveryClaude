using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lib.Cosmos.Apis.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Operators;
using Lib.Cosmos.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Apis;

/// <summary>
/// Provides an adapter for Cosmos DB container operations.
/// </summary>
public abstract class CosmosContainerAdapter : ICosmosContainerAdapter
{
    private readonly ICosmosContainerQueryOperator _query;
    private readonly ICosmosContainerQueryAsyncOperator _queryAsync;
    private readonly ICosmosContainerDeleteOperator _delete;
    private readonly ICosmosContainerReadOperator _readItem;
    private readonly ICosmosContainerUpsertOperator _upsert;

    /// <summary>
    /// Initializes a new instance of the <see cref="CosmosContainerAdapter"/> class with logger, container config, and connection config.
    /// </summary>
    protected CosmosContainerAdapter(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience) : this(
        new CosmosContainerQueryOperator(logger, containerDefinition, connectionConvenience),
        new CosmosContainerQueryAsyncOperator(logger, containerDefinition, connectionConvenience),
        new CosmosContainerDeleteOperator(logger, containerDefinition, connectionConvenience),
        new CosmosContainerReadOperator(logger, containerDefinition, connectionConvenience),
        new CosmosContainerUpsertOperator(logger, containerDefinition, connectionConvenience))
    { }

    private CosmosContainerAdapter(
        ICosmosContainerQueryOperator query,
        ICosmosContainerQueryAsyncOperator queryAsync,
        ICosmosContainerDeleteOperator delete,
        ICosmosContainerReadOperator readItem,
        ICosmosContainerUpsertOperator upsert)
    {
        _query = query;
        _queryAsync = queryAsync;
        _delete = delete;
        _readItem = readItem;
        _upsert = upsert;
    }

    /// <summary>
    /// Executes a query against the container.
    /// </summary>
    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(QueryDefinition queryDefinition, PartitionKey partitionKey, CancellationToken cancellationToken = default)
        => await _query.QueryAsync<T>(queryDefinition, partitionKey, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Deletes an item from the container.
    /// </summary>
    public async Task<OpResponse<T>> DeleteAsync<T>(DeletePointItem item)
        => await _delete.DeleteAsync<T>(item).ConfigureAwait(false);

    /// <summary>
    /// Reads an item from the container.
    /// </summary>
    public async Task<OpResponse<T>> ReadAsync<T>(ReadPointItem item)
        => await _readItem.ReadAsync<T>(item).ConfigureAwait(false);

    /// <summary>
    /// Upserts an item into the container.
    /// </summary>
    public async Task<OpResponse<T>> UpsertAsync<T>(T item)
        => await _upsert.UpsertAsync<T>(item).ConfigureAwait(false);

    /// <summary>
    /// Executes a query against the container.
    /// </summary>
    public IAsyncEnumerable<OpResponse<T>> QueryYield<T>(QueryDefinition queryDefinition, PartitionKey partitionKey, CancellationToken cancellationToken = default)
        => _queryAsync.QueryYield<T>(queryDefinition, partitionKey, cancellationToken);
}

