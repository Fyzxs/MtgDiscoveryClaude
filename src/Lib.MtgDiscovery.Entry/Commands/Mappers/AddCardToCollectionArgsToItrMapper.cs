using System.Collections.Generic;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Commands.Mappers;

internal sealed class AddCardToCollectionArgsToItrMapper : IAddCardToCollectionArgsToItrMapper
{
    public Task<IUserCardCollectionItrEntity> Map(IAuthUserArgEntity source1, IAddCardToCollectionArgEntity source2)
    {
        // Convert single item to collection for pipeline compatibility
        List<ICollectedItemItrEntity> collectedList = new List<ICollectedItemItrEntity>
        {
            MapToCollectedItemItrEntity(source2.CollectedItem)
        };

        // Combine user ID from auth with args data
        return Task.FromResult<IUserCardCollectionItrEntity>(new UserCardCollectionItrEntity
        {
            UserId = source1.UserId,  // Get UserId from JWT auth
            CardId = source2.CardId,
            SetId = source2.SetId,
            CollectedList = collectedList
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
