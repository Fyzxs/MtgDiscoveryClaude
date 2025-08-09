using System;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Operators;

internal sealed class CosmosContainerUpsertOperator : ICosmosContainerUpsertOperator
{
    private readonly ILogger _logger;
    private readonly ICosmosClientAdapter _clientAdapter;

    public CosmosContainerUpsertOperator(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
        : this(logger, new MonoStateCosmosClientAdapter(logger, containerDefinition, connectionConvenience))
    { }

    private CosmosContainerUpsertOperator(ILogger logger, ICosmosClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }
    public async Task<OpResponse<T>> UpsertAsync<T>(T item)
    {
        Container container = await _clientAdapter.GetContainer().ConfigureAwait(false);
        ItemResponse<T> itemResponse = await container.UpsertItemAsync(item).ConfigureAwait(false);
        _logger.UpsertInformation(itemResponse.RequestCharge, itemResponse.Diagnostics.GetClientElapsedTime());
        return new ItemOpResponse<T>(itemResponse);
    }
}

internal static partial class CosmosContainerUpsertAdapterLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "UpsertItem cost: [RequestCharge={requestCharge}] [ElapsedTime={elapsedTime}]")]
    public static partial void UpsertInformation(this ILogger logger, double requestCharge, TimeSpan elapsedTime);
}

