using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal sealed class UserCardsSetXfrToArgsMapper : IUserCardsSetXfrToArgsMapper
{
    public Task<UserCardItemsBySetExtEntitys> Map(IUserCardsSetXfrEntity source)
    {
        UserCardItemsBySetExtEntitys args = new()
        {
            UserId = source.UserId,
            SetId = source.SetId
        };

        return Task.FromResult(args);
    }
}
