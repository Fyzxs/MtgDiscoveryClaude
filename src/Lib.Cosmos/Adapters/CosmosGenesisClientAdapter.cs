using System;
using System.Threading.Tasks;
using Azure.Core;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Adapters;

internal abstract class CosmosGenesisClientAdapter : ICosmosGenesisClientAdapter
{
    private readonly CosmosClient _cosmosClient;

    protected CosmosGenesisClientAdapter(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    protected static CosmosClientOptions Options(ICosmosConnectionOptionsConfig config)
        => new()
        {
            ConnectionMode = config.ConnectionMode(),
            ApplicationPreferredRegions = config.PreferredRegions().AsSystemType()
        };

    public Task<Container> GetContainer(ICosmosContainerDefinition cosmosContainerDef)
        => Task.FromResult(_cosmosClient.GetContainer(cosmosContainerDef.DatabaseName(), cosmosContainerDef.ContainerName()));

    public async Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition)
        => await InternalCreateDatabaseIfNotExistsAsync(containerDefinition, _cosmosClient).ConfigureAwait(false);

    protected abstract Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient);
}

internal sealed class EntraAuthCosmosGenesisClientAdapter : CosmosGenesisClientAdapter
{
    public EntraAuthCosmosGenesisClientAdapter(ICosmosContainerDefinition cosmosContainerDef, ICosmosConnectionConvenience connectionConvenience)
        : base(NewCosmosClient(cosmosContainerDef, connectionConvenience))
    { }

    private static CosmosClient NewCosmosClient(ICosmosContainerDefinition cosmosContainerDef, ICosmosConnectionConvenience connectionConvenience)
    {
        CosmosAccountEndpoint accountEndpoint = connectionConvenience.AccountConfig(cosmosContainerDef).EntraConfig().ConnectionConfig().AccountEndpoint();
        TokenCredential tokenCredentials = connectionConvenience.AccountEntraCredential(cosmosContainerDef);
        CosmosClientOptions options = Options(connectionConvenience.AccountConfig(cosmosContainerDef).EntraConfig().ConnectionConfig());

        return new CosmosClient(accountEndpoint, tokenCredentials, options);
    }

    protected override Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient)
        => throw new NotImplementedException("Cannot Genesis this way when using Entra Auth");
}

internal sealed class SasAuthCosmosGenesisClientAdapter : CosmosGenesisClientAdapter
{
    public SasAuthCosmosGenesisClientAdapter(ICosmosSasConnectionConfig config)
        : base(new CosmosClient(config.ConnectionString(), Options(config)))
    { }

    protected override async Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient)
        => await cosmosClient.CreateDatabaseIfNotExistsAsync(containerDefinition.DatabaseName()).ConfigureAwait(false);
}
