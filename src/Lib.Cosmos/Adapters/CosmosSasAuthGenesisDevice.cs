using System.Threading.Tasks;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Adapters;

internal sealed class CosmosSasAuthGenesisDevice : IGenesisDevice
{
    private readonly ICosmosContainerDefinition _cosmosContainerDefinition;
    private readonly ICosmosConnectionConvenience _connectionConvenience;

    public CosmosSasAuthGenesisDevice(ICosmosContainerDefinition cosmosContainerDefinition, ICosmosConnectionConvenience connectionConvenience)
    {
        _cosmosContainerDefinition = cosmosContainerDefinition;
        _connectionConvenience = connectionConvenience;
    }

    public async Task LiveLongAndProsper(ICosmosGenesisClientAdapter genesisClientAdapter)
    {
        DatabaseResponse databaseResponse = await genesisClientAdapter.CreateDatabaseIfNotExistsAsync(_cosmosContainerDefinition).ConfigureAwait(false);
        ICosmosContainerConfig containerConfig = _connectionConvenience.ContainerConfig(_cosmosContainerDefinition);

        ContainerProperties containerProperties = new()
        {
            Id = _cosmosContainerDefinition.ContainerName(),
            PartitionKeyPath = _cosmosContainerDefinition.PartitionKeyPath(),
            DefaultTimeToLive = containerConfig.TimeToLive()
        };
        ThroughputProperties throughputProperties = ThroughputProperties.CreateAutoscaleThroughput(containerConfig.AutoscaleMax());

        await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerProperties, throughputProperties).ConfigureAwait(false);
    }
}
