using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemMapper : ICollectedItemMapper
{
    public CollectedItem Map(ICollectedItemItrEntity collected)
    {
        return new CollectedItem
        {
            Finish = collected.Finish,
            Special = collected.Special,
            Count = collected.Count
        };
    }
}
