using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Itrs;
using Lib.Shared.Invocation.Operations;

namespace Cli.MtgDiscovery.DataMigration.NewSystem;

internal interface INewSystemCardLookup
{
    Task<IOperationResponse<ICardItemItrEntity>> LookupCardByScryfallIdAsync(string scryfallId);
}
