using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Cosmos.Adapters;
using Lib.Cosmos.Apis.Configurations;
using Lib.Cosmos.Apis.Operators;
using Lib.Universal.Extensions;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Operators;

internal sealed class CosmosContainerReadOperator : ICosmosContainerReadOperator
{
    private readonly ILogger _logger;
    private readonly ICosmosClientAdapter _clientAdapter;

    public CosmosContainerReadOperator(ILogger logger, ICosmosContainerDefinition containerDefinition, ICosmosConnectionConvenience connectionConvenience)
        : this(logger, new MonoStateCosmosClientAdapter(logger, containerDefinition, connectionConvenience))
    { }

    private CosmosContainerReadOperator(ILogger logger, ICosmosClientAdapter clientAdapter)
    {
        _logger = logger;
        _clientAdapter = clientAdapter;
    }

    public async Task<OpResponse<T>> ReadAsync<T>([NotNull] ReadPointItem item)
    {
        Container container = await _clientAdapter.GetContainer().ConfigureAwait(false);
        try
        {
            ItemResponse<T> itemResponse = await container.ReadItemAsync<T>(item.Id, item.Partition).ConfigureAwait(false);
            _logger.ReadInformation(itemResponse.RequestCharge, itemResponse.Diagnostics.GetClientElapsedTime());
            return new ItemOpResponse<T>(itemResponse);
        }
        catch (CosmosException ex)
        {
            _logger.ReadInformation(ex.RequestCharge, ex.Diagnostics.GetClientElapsedTime());
            return new CosmosExceptionOpResponse<T>(ex);
        }
#pragma warning disable CA1031
        catch (Exception ex)
#pragma warning restore CA1031
        {
            throw ex.ThrowMe();
        }
    }
}

internal static partial class CosmosContainerReadItemAdapterLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "ReadItem cost: [RequestCharge={requestCharge}] [ElapsedTime={elapsedTime}]")]
    public static partial void ReadInformation(this ILogger logger, double requestCharge, TimeSpan elapsedTime);
}
