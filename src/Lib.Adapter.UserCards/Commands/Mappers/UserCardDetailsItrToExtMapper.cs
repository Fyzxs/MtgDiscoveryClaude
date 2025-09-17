using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Adapter.UserCards.Commands.Mappers;

internal sealed class UserCardDetailsItrToExtMapper : IUserCardDetailsItrToExtMapper
{
    public Task<UserCardDetailsExtEntity> Map(IUserCardDetailsItrEntity collected)
    {
        return Task.FromResult(new UserCardDetailsExtEntity
        {
            Finish = collected.Finish,
            Special = collected.Special,
            Count = collected.Count
        });
    }
}
