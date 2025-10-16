using System.Threading.Tasks;
using Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;
using Lib.Cosmos.Apis.Operators;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Operators;

public interface IDiscoveryCardGopher
{
    Task<OpResponse<OldDiscoveryCardExtEntity>> ReadCardAsync(string cardId);
}
