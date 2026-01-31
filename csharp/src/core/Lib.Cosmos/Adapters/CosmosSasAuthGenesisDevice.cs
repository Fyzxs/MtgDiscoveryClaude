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
        ICosmosDatabaseConfig databaseConfig = _connectionConvenience.DatabaseConfig(_cosmosContainerDefinition);
        ICosmosContainerConfig containerConfig = databaseConfig.ContainerConfig(_cosmosContainerDefinition);
        ICosmosThroughputMode throughputMode = databaseConfig.ThroughputMode();

        DatabaseResponse databaseResponse = await CreateDatabase(genesisClientAdapter, databaseConfig, throughputMode).ConfigureAwait(false);
        await CreateContainer(databaseResponse, containerConfig, throughputMode).ConfigureAwait(false);
    }

    private async Task<DatabaseResponse> CreateDatabase(ICosmosGenesisClientAdapter genesisClientAdapter, ICosmosDatabaseConfig databaseConfig, ICosmosThroughputMode throughputMode)
    {
        ThroughputProperties throughputProperties = throughputMode.IsDatabaseShared()
            ? ThroughputProperties.CreateAutoscaleThroughput(databaseConfig.AutoscaleMax())
            : null;

        return await genesisClientAdapter.CreateDatabaseIfNotExistsAsync(_cosmosContainerDefinition, throughputProperties).ConfigureAwait(false);
    }

    private async Task CreateContainer(DatabaseResponse databaseResponse, ICosmosContainerConfig containerConfig, ICosmosThroughputMode throughputMode)
    {
        ContainerProperties containerProperties = new()
        {
            Id = _cosmosContainerDefinition.ContainerName(),
            PartitionKeyPath = _cosmosContainerDefinition.PartitionKeyPath(),
            DefaultTimeToLive = containerConfig.TimeToLive()
        };

        if (throughputMode.IsDatabaseShared())
        {
            await CreateContainerWithoutThroughput(databaseResponse, containerProperties).ConfigureAwait(false);
        }
        else
        {
            await CreateContainerWithThroughput(databaseResponse, containerProperties, containerConfig).ConfigureAwait(false);
        }
    }

    private static async Task CreateContainerWithoutThroughput(DatabaseResponse databaseResponse, ContainerProperties containerProperties)
        => await databaseResponse.Database.CreateContainerIfNotExistsAsync(containerProperties).ConfigureAwait(false);

    private static async Task CreateContainerWithThroughput(DatabaseResponse databaseResponse, ContainerProperties containerProperties, ICosmosContainerConfig containerConfig)
    {
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
