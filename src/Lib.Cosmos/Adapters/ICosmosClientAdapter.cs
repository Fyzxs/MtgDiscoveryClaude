using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace Lib.Cosmos.Adapters;

internal interface ICosmosClientAdapter
{
    Task<Container> GetContainer();
}