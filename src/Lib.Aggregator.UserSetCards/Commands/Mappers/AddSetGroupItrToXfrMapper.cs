using System.Threading.Tasks;
using Lib.Aggregator.UserSetCards.Commands.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserSetCards;
using Lib.Shared.DataModels.Entities.Xfrs.UserSetCards;

namespace Lib.Aggregator.UserSetCards.Commands.Mappers;

internal sealed class AddSetGroupItrToXfrMapper : IAddSetGroupItrToXfrMapper
{
    public Task<IAddSetGroupToUserSetCardXfrEntity> Map(IAddSetGroupToUserSetCardItrEntity source)
    {
        return Task.FromResult<IAddSetGroupToUserSetCardXfrEntity>(new AddSetGroupToUserSetCardXfrEntity
        {
            UserId = source.UserId,
            SetId = source.SetId,
            SetGroupId = source.SetGroupId,
            Collecting = source.Collecting,
            Count = source.Count
        });
    }
}
