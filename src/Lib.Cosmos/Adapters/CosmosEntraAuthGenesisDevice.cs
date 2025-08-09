using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using Azure.ResourceManager.Resources;
using Lib.Cosmos.Apis.Configurations;
using Lib.Universal.Extensions;
using Microsoft.Extensions.Logging;

namespace Lib.Cosmos.Adapters;

internal sealed class CosmosEntraAuthGenesisDevice : IGenesisDevice
{
    private readonly ILogger _logger;
    private readonly ICosmosContainerDefinition _containerConfig;
    private readonly ICosmosConnectionConvenience _connectionConfig;
    private readonly ICosmosArmClientAdapter _client;

    public CosmosEntraAuthGenesisDevice(ILogger logger, ICosmosContainerDefinition containerConfig, ICosmosConnectionConvenience connectionConfig)
        : this(logger, containerConfig, connectionConfig, new MonoStateCosmosArmClientAdapter(containerConfig, connectionConfig))
    { }

    private CosmosEntraAuthGenesisDevice(ILogger logger, ICosmosContainerDefinition containerConfig, ICosmosConnectionConvenience connectionConfig, ICosmosArmClientAdapter client)
    {
        _logger = logger;
        _containerConfig = containerConfig;
        _connectionConfig = connectionConfig;
        _client = client;
    }

    public async Task LiveLongAndProsper(ICosmosGenesisClientAdapter genesisClientAdapter)
    {
        ICosmosEntraGenesisConfig genesisConfig = _connectionConfig.EntraGenesisConfig(_containerConfig);
        if (genesisConfig.Bypass()) return;

        try
        {
            await Genesis(genesisConfig).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.LogCreateOrUpdateError(
                ex,
                _containerConfig.DatabaseName(),
                _containerConfig.ContainerName(),
                genesisConfig.SubscriptionId(),
                genesisConfig.ResourceGroupName(),
                _connectionConfig.AccountConfig(_containerConfig).AccountName(),
                _containerConfig.PartitionKeyPath(),
                _connectionConfig.ContainerConfig(_containerConfig).AutoscaleMax()
            );
            throw ex.ThrowMe();
        }
    }

    private async Task Genesis(ICosmosEntraGenesisConfig genesisConfig)
    {
        _logger.LogCreateOrUpdateDebug(
            _containerConfig.DatabaseName(),
            _containerConfig.ContainerName()
        );

        CosmosDBAccountResource dbAccount = await CosmosAccount(genesisConfig).ConfigureAwait(false);
        AzureLocation location = dbAccount.Data.Location;
        CosmosDBSqlDatabaseResource cosmosDbSqlResource = await CreateDatabase(dbAccount, location).ConfigureAwait(false);
        await CreateCollection(cosmosDbSqlResource, location, _containerConfig).ConfigureAwait(false);
    }

    private async Task<CosmosDBAccountResource> CosmosAccount(ICosmosEntraGenesisConfig genesisConfig)
    {
        SubscriptionResource targetSubscription = _client.GetSubscriptionResource(SubscriptionResource.CreateResourceIdentifier(genesisConfig.SubscriptionId()));
        ResourceGroupResource rg = (await targetSubscription.GetResourceGroupAsync(genesisConfig.ResourceGroupName()).ConfigureAwait(false)).Value;
        CosmosDBAccountResource dbAccount = (await rg.GetCosmosDBAccountAsync(_connectionConfig.AccountConfig(_containerConfig).AccountName()).ConfigureAwait(false)).Value;
        return dbAccount;
    }

    private async Task<CosmosDBSqlDatabaseResource> CreateDatabase(CosmosDBAccountResource dbAccount, AzureLocation location)
    {
        CosmosDBSqlDatabaseCollection databases = dbAccount.GetCosmosDBSqlDatabases();
        CosmosDBSqlDatabaseResourceInfo cosmosDbSqlDatabaseResourceInfo = new(_containerConfig.DatabaseName());
        CosmosDBSqlDatabaseCreateOrUpdateContent createOrUpdateContent = new(location, cosmosDbSqlDatabaseResourceInfo);

        if (await databases.ExistsAsync(_containerConfig.DatabaseName()).ConfigureAwait(false)) return await databases.GetAsync(_containerConfig.DatabaseName()).ConfigureAwait(false);

        return (await databases.CreateOrUpdateAsync(WaitUntil.Completed, _containerConfig.DatabaseName(), createOrUpdateContent).ConfigureAwait(false)).Value;
    }

    private async Task CreateCollection(CosmosDBSqlDatabaseResource cosmosDbSqlResource, AzureLocation location, ICosmosContainerDefinition cosmosContainerConfig)
    {
        CosmosDBSqlContainerCollection cosmosDbSqlContainers = cosmosDbSqlResource.GetCosmosDBSqlContainers();
        if (await cosmosDbSqlContainers.ExistsAsync(cosmosContainerConfig.ContainerName()).ConfigureAwait(false)) return;
#pragma warning disable S1481 //_ is fine
        ArmOperation<CosmosDBSqlContainerResource> _ = await cosmosDbSqlContainers.CreateOrUpdateAsync(WaitUntil.Completed, cosmosContainerConfig.ContainerName(), CollectionConfig(location)).ConfigureAwait(false);
#pragma warning restore S1481
    }

    private CosmosDBSqlContainerCreateOrUpdateContent CollectionConfig(AzureLocation location)
    {
        ICosmosContainerConfig containerConfig = _connectionConfig.ContainerConfig(_containerConfig);
        CosmosDBContainerPartitionKey cosmosDbContainerPartitionKey = new() { Kind = CosmosDBPartitionKind.Hash };
        cosmosDbContainerPartitionKey.Paths.Add(_containerConfig.PartitionKeyPath());

        CosmosDBSqlContainerResourceInfo cosmosDbSqlContainerResourceInfo = new(_containerConfig.ContainerName())
        {
            PartitionKey = cosmosDbContainerPartitionKey,
            DefaultTtl = containerConfig.TimeToLive()
        };

        CosmosDBSqlContainerCreateOrUpdateContent cosmosDbSqlContainerCreateOrUpdateContent = new(location, cosmosDbSqlContainerResourceInfo)
        {
            Options = new CosmosDBCreateUpdateConfig()
            {
                AutoscaleMaxThroughput = containerConfig.AutoscaleMax()
            }
        };

        return cosmosDbSqlContainerCreateOrUpdateContent;
    }
}

internal static partial class AadAuthGenesisDeviceLoggerExtensions
{
    [LoggerMessage(
        Level = LogLevel.Debug,
        Message = "Performing CreateOrUpdate on [db={databaseName}]/[container={containerName}]")]
    public static partial void LogCreateOrUpdateDebug(this ILogger logger, string databaseName, string containerName);

#pragma warning disable S107 // 7 params max
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Failure performing CreateOrUpdate on [db={databaseName}]/[container={containerName}]\nwith configuration [subscription={subscription}] [resourceGroup={resourceGroup}] [accountName={accountName}]\nwith settings [partitionKeyPath={partitionKeyPath}] [autoscaleMax={autoscaleMax}]")]
    public static partial void LogCreateOrUpdateError(this ILogger logger, Exception exception, string databaseName, string containerName, string subscription, string resourceGroup, string accountName, string partitionKeyPath, int autoscaleMax);
#pragma warning restore S107
}
