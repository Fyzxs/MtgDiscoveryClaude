using Azure.Core;
using Lib.Cosmos.Apis.Configurations;

namespace Lib.Cosmos.Tests.Fakes;

internal sealed class CosmosConnectionConvenienceFake : ICosmosConnectionConvenience
{
    public ICosmosAccountConfig AccountConfigResult { get; init; }
    public int AccountConfigInvokeCount { get; private set; }

    public TokenCredential AccountEntraCredentialResult { get; init; }
    public int AccountEntraCredentialInvokeCount { get; private set; }

    public ICosmosEntraGenesisConfig EntraGenesisConfigResult { get; init; }
    public int EntraGenesisConfigInvokeCount { get; private set; }

    public ICosmosDatabaseConfig DatabaseConfigResult { get; init; }
    public int DatabaseConfigInvokeCount { get; private set; }

    public ICosmosContainerConfig ContainerConfigResult { get; init; }
    public int ContainerConfigInvokeCount { get; private set; }

    public ICosmosAccountConfig AccountConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        AccountConfigInvokeCount++;
        return AccountConfigResult;
    }

    public TokenCredential AccountEntraCredential(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        AccountEntraCredentialInvokeCount++;
        return AccountEntraCredentialResult;
    }

    public ICosmosEntraGenesisConfig EntraGenesisConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        EntraGenesisConfigInvokeCount++;
        return EntraGenesisConfigResult;
    }

    public ICosmosDatabaseConfig DatabaseConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        DatabaseConfigInvokeCount++;
        return DatabaseConfigResult;
    }

    public ICosmosContainerConfig ContainerConfig(ICosmosContainerDefinition cosmosContainerDefinition)
    {
        ContainerConfigInvokeCount++;
        return ContainerConfigResult;
    }
}
