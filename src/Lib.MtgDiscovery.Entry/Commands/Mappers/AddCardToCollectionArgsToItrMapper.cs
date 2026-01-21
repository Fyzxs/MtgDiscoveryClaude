using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal sealed class AddCardToCollectionArgsToItrMapper : IAddCardToCollectionArgsToItrMapper
{
    public Task<IUserCardCollectionItrEntity> Map(IAddCardToCollectionArgEntity source)
    {
        return Task.FromResult<IUserCardCollectionItrEntity>(new UserCardCollectionItrEntity
        {
            UserId = source.UserId,
            CardId = source.CardId,
            SetId = source.SetId,
            CollectedList = source.CollectedList.Select(MapToCollectedItemItrEntity).ToList()
        });
    }

    private static ICollectedItemItrEntity MapToCollectedItemItrEntity(ICollectedItemArgEntity argItem)
    {
        return new CollectedItemItrEntity
        {
            Finish = argItem.Finish,
            Special = argItem.Special,
            Count = argItem.Count
        };
    }
}
