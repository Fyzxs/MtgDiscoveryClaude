using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Adapter.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemItrEntityMapper : ICollectedItemItrEntityMapper
{
    public Task<ICollectedItemItrEntity> Map(CollectedItem collectedItem)
    {
        return Task.FromResult<ICollectedItemItrEntity>(new CollectedItemItrEntity
        {
            Finish = collectedItem.Finish,
            Special = collectedItem.Special,
            Count = collectedItem.Count
        });
    }
}
