using System;
using System.Diagnostics.CodeAnalysis;
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

        try
        {
            await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerProperties, throughputProperties).ConfigureAwait(false);
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.BadRequest && ex.ResponseBody.StartsWith("Setting offer throughput or autopilot on container is not supported for serverless accounts."))
        {
            await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerProperties).ConfigureAwait(false);
        }
    }
}
