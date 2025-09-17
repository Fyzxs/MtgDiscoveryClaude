using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemMapper : ICollectedItemMapper
{
    public Task<CollectedCardInfoExtArg> Map(ICollectedItemItrEntity collected)
    {
        return Task.FromResult(new CollectedCardInfoExtArg
        {
            Finish = collected.Finish,
            Special = collected.Special,
            Count = collected.Count
        });
    }
}
