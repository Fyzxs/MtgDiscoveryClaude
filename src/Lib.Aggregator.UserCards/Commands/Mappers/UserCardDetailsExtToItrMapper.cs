using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.UserCards.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Commands.Mappers;

internal sealed class UserCardDetailsExtToOufMapper : IUserCardDetailsExtToOufMapper
{
    public Task<IUserCardDetailsOufEntity> Map(UserCardDetailsExtEntity source)
    {
        IUserCardDetailsOufEntity result = new UserCardDetailsOufEntity
        {
            Finish = source.Finish,
            Special = source.Special,
            Count = source.Count
        };

        return Task.FromResult(result);
    }
}
