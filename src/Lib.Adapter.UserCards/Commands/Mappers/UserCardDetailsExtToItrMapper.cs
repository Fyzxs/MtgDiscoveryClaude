using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Adapter.UserCards.Commands.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardDetailsExtToItrMapper : IUserCardDetailsExtToItrMapper
{
    public Task<IUserCardDetailsItrEntity> Map(UserCardDetailsExtEntity userCardDetailsExtEntity)
    {
        return Task.FromResult<IUserCardDetailsItrEntity>(new UserCardDetailsItrEntity
        {
            Finish = userCardDetailsExtEntity.Finish,
            Special = userCardDetailsExtEntity.Special,
            Count = userCardDetailsExtEntity.Count
        });
    }
}
