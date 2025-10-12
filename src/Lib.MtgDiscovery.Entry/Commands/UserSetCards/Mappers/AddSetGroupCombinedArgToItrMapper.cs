using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Commands.Entities;
using Lib.MtgDiscovery.Entry.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;

namespace Lib.MtgDiscovery.Entry.Commands.UserSetCards.Mappers;

internal sealed class AddSetGroupCombinedArgToItrMapper : IAddSetGroupCombinedArgToItrMapper
{
    public Task<IAddSetGroupToUserSetCardItrEntity> Map(IAddSetGroupToUserSetCardArgsEntity from)
    {
        AddSetGroupToUserSetCardItrEntity itrEntity = new()
        {
            UserId = from.AuthUser.UserId,
            SetId = from.AddSetGroupToUserSetCard.SetId,
            SetGroupId = from.AddSetGroupToUserSetCard.SetGroupId,
            Collecting = from.AddSetGroupToUserSetCard.Collecting,
            Count = from.AddSetGroupToUserSetCard.Count
        };

        return Task.FromResult<IAddSetGroupToUserSetCardItrEntity>(itrEntity);
    }
}
