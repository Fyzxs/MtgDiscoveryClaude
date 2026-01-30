using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Operators;

internal sealed class CosmosContainerQueryAsyncOperator : ICosmosContainerQueryAsyncOperator
{
    private readonly ICosmosClientAdapter _clientAdapter;
    private readonly ILogger _logger;

    public CosmosContainerQueryAsyncOperator(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
        : this(logger, new MonoStateCosmosClientAdapter(logger, containerDefinition, connectionConvenience))
    { }

    private CosmosContainerQueryAsyncOperator(ILogger logger, ICosmosClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async IAsyncEnumerable<OpResponse<T>> QueryYield<T>(QueryDefinition queryDefinition, PartitionKey partitionKey, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        Container container = await _clientAdapter.GetContainer().ConfigureAwait(false);
        QueryRequestOptions requestOptions = new() { PartitionKey = partitionKey };
        FeedIterator<T> iterator = container.GetItemQueryIterator<T>(queryDefinition, requestOptions: requestOptions);

        while (iterator.HasMoreResults)
        {
            FeedResponse<T> response = await iterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
            _logger.QueryYieldInformation(response.RequestCharge, response.Diagnostics.GetClientElapsedTime(), response.Count);

            foreach (T item in response)
            {
                yield return new ProvidedOpResponse<T>(item, response.StatusCode);
            }
        }
    }
}

internal static partial class CosmosContainerQueryAsyncOperatorLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "QueryYield batch: [RequestCharge={requestCharge}] [ElapsedTime={elapsedTime}] [ItemCount={itemCount}]")]
    public static partial void QueryYieldInformation(this ILogger logger, double requestCharge, TimeSpan elapsedTime, int itemCount);
}
