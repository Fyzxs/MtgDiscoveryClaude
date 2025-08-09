using System.Threading.Tasks;

namespace Lib.Cosmos.Adapters;

internal interface IGenesisDevice
{
    Task LiveLongAndProsper(ICosmosGenesisClientAdapter genesisClientAdapter);
}
