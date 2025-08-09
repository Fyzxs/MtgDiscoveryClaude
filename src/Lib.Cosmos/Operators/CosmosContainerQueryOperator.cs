using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Operators;

internal sealed class CosmosContainerQueryOperator : ICosmosContainerQueryOperator
{
    private readonly ICosmosClientAdapter _clientAdapter;
    private readonly ILogger _logger;

    public CosmosContainerQueryOperator(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
        : this(logger, new MonoStateCosmosClientAdapter(logger, containerDefinition, connectionConvenience))
    { }

    private CosmosContainerQueryOperator(ILogger logger, ICosmosClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task<OpResponse<IEnumerable<T>>> QueryAsync<T>(QueryDefinition queryDefinition, PartitionKey partitionKey, CancellationToken cancellationToken = default)
    {
        Container container = await _clientAdapter.GetContainer().ConfigureAwait(false);
        FeedIterator<T> iterator = container.GetItemQueryIterator<T>(queryDefinition);
        List<T> collection = [];

        HttpStatusCode highestStatusCode = HttpStatusCode.OK;
        while (iterator.HasMoreResults)
        {
            FeedResponse<T> response = await iterator.ReadNextAsync(cancellationToken).ConfigureAwait(false);
            if (highestStatusCode < response.StatusCode) highestStatusCode = response.StatusCode;
            _logger.QueryInformation(response.RequestCharge, response.Diagnostics.GetClientElapsedTime());
            collection.AddRange(response.Resource);
        }

        return new ProvidedOpResponse<IEnumerable<T>>(collection, highestStatusCode);
    }
}

internal static partial class CosmosContainerQueryAdapterLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "GetItemQueryIterator cost: [RequestCharge={requestCharge}] [ElapsedTime={elapsedTime}]")]
    public static partial void QueryInformation(this ILogger logger, double requestCharge, TimeSpan elapsedTime);
}
