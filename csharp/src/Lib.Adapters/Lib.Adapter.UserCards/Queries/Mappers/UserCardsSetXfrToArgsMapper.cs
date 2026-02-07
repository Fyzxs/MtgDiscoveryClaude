using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal sealed class UserCardsSetXfrToArgsMapper : IUserCardsSetXfrToArgsMapper
{
    public Task<UserCardItemsBySetExtEntity> Map(IUserCardsSetXfrEntity source)
    {
        UserCardItemsBySetExtEntity args = new()
        {
            UserId = source.UserId,
            SetId = source.SetId
        };

        return Task.FromResult(args);
    }
}
