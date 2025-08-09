using System;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Operators;

internal sealed class CosmosContainerDeleteOperator : ICosmosContainerDeleteOperator
{
    private readonly ILogger _logger;
    private readonly ICosmosClientAdapter _clientAdapter;

    public CosmosContainerDeleteOperator(ILogger logger, ICosmosContainerDefinition containerConfig, ICosmosConnectionConvenience connectionConfig)
        : this(logger, new MonoStateCosmosClientAdapter(logger, containerConfig, connectionConfig))
    { }

    private CosmosContainerDeleteOperator(ILogger logger, ICosmosClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task<OpResponse<T>> DeleteAsync<T>(DeletePointItem item)
    {
        Container container = await _clientAdapter.GetContainer().ConfigureAwait(false);
        ItemResponse<T> itemResponse = await container.DeleteItemAsync<T>(item.Id, item.Partition).ConfigureAwait(false);
        _logger.DeleteInformation(itemResponse.RequestCharge, itemResponse.Diagnostics.GetClientElapsedTime());
        return new ItemOpResponse<T>(itemResponse);
    }
}
internal static partial class CosmosContainerDeleteAdapterLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "DeleteItem cost: [RequestCharge={requestCharge}] [ElapsedTime={elapsedTime}]")]
    public static partial void DeleteInformation(this ILogger logger, double requestCharge, TimeSpan elapsedTime);
}
