using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal interface ICollectedItemItrEntityMapper
{
    ICollectedItemItrEntity Map(CollectedItem collectedItem);
}

internal sealed class CollectedItemItrEntityMapper : ICollectedItemItrEntityMapper
{
    public ICollectedItemItrEntity Map(CollectedItem collectedItem)
    {
        return new CollectedItemItrEntity
        {
            Finish = collectedItem.Finish,
            Special = collectedItem.Special,
            Count = collectedItem.Count
        };
    }
}

internal sealed class CollectedItemItrEntity : ICollectedItemItrEntity
{
    public string Finish { get; init; }
    public string Special { get; init; }
    public int Count { get; init; }
}
