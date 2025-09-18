using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal sealed class UserCardDetailsExtToItrMapper : IUserCardDetailsExtToItrMapper
{
    public Task<IUserCardDetailsItrEntity> Map(UserCardDetailsExtEntity source)
    {
        IUserCardDetailsItrEntity result = new UserCardDetailsItrEntity
        {
            Finish = source.Finish,
            Special = source.Special,
            Count = source.Count
        };

        return Task.FromResult(result);
    }
}