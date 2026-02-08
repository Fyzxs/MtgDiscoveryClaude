using System.Threading.Tasks;
using Lib.Shared.DataModels.Entities.Oufs.Cards;
using Lib.Shared.Invocation.Operations;

namespace Cli.MtgDiscovery.DataMigration.NewSystem;

internal interface INewSystemCardLookup
{
    Task<IOperationResponse<ICardItemOufEntity>> LookupCardByScryfallIdAsync(string scryfallId);
}
