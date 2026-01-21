using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemMapper : ICollectedItemMapper
{
    public Task<CollectedItem> Map(ICollectedItemItrEntity collected)
    {
        return Task.FromResult(new CollectedItem
        {
            Finish = collected.Finish,
            Special = collected.Special,
            Count = collected.Count
        });
    }
}
