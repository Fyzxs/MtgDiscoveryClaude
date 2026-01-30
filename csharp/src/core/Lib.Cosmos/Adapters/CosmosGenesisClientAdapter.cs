using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Core;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Adapters;

internal abstract class CosmosGenesisClientAdapter : ICosmosGenesisClientAdapter
{
    private readonly CosmosClient _cosmosClient;

    protected CosmosGenesisClientAdapter(CosmosClient cosmosClient) => _cosmosClient = cosmosClient;

    protected static CosmosClientOptions Options(ICosmosConnectionOptionsConfig config)
    {
        CosmosClientOptions options = new()
        {
            ConnectionMode = config.ConnectionMode(),
            ApplicationPreferredRegions = config.PreferredRegions().AsSystemType(),
            HttpClientFactory = () =>
            {
                HttpMessageHandler httpMessageHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (_, cert, _, sslPolicyErrors) =>
                    {
                        if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
                        {
                            return true;
                        }

                        string issuer = cert?.Issuer ?? string.Empty;
                        return issuer.Contains("localhost", StringComparison.OrdinalIgnoreCase);
                    }
                };
                return new HttpClient(httpMessageHandler);
            }
        };
        return options;
    }

    public Task<Container> GetContainer(ICosmosContainerDefinition cosmosContainerDef)
        => Task.FromResult(_cosmosClient.GetContainer(cosmosContainerDef.DatabaseName(), cosmosContainerDef.ContainerName()));

    public async Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, ThroughputProperties throughputProperties)
        => await InternalCreateDatabaseIfNotExistsAsync(containerDefinition, _cosmosClient, throughputProperties).ConfigureAwait(false);

    protected abstract Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient, ThroughputProperties throughputProperties);
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

    protected override Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient, ThroughputProperties throughputProperties)
        => throw new NotImplementedException("Cannot Genesis this way when using Entra Auth");
}

internal sealed class SasAuthCosmosGenesisClientAdapter : CosmosGenesisClientAdapter
{
    public SasAuthCosmosGenesisClientAdapter(ICosmosSasConnectionConfig config)
        : base(new CosmosClient(config.ConnectionString(), Options(config)))
    { }

    protected override async Task<DatabaseResponse> InternalCreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition, CosmosClient cosmosClient, ThroughputProperties throughputProperties)
        => await cosmosClient.CreateDatabaseIfNotExistsAsync(containerDefinition.DatabaseName(), throughputProperties).ConfigureAwait(false);
}
