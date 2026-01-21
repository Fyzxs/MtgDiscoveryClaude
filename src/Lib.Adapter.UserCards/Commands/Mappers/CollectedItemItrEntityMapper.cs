using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class CollectedItemItrEntityMapper : ICollectedItemItrEntityMapper
{
    public Task<ICollectedItemItrEntity> Map(CollectedCardInfoExtArg collectedCardInfoExtArg)
    {
        return Task.FromResult<ICollectedItemItrEntity>(new CollectedItemItrEntity
        {
            Finish = collectedCardInfoExtArg.Finish,
            Special = collectedCardInfoExtArg.Special,
            Count = collectedCardInfoExtArg.Count
        });
    }
}
