using System.Threading.Tasks;
using Lib.Cosmos.Apis.Configurations;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Adapters;

internal interface ICosmosGenesisClientAdapter
{
    Task<Container> GetContainer(ICosmosContainerDefinition cosmosContainerDef);

    Task<DatabaseResponse> CreateDatabaseIfNotExistsAsync(ICosmosContainerDefinition containerDefinition);
}
