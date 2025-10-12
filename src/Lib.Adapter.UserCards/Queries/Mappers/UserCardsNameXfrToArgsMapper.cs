using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal sealed class UserCardsNameXfrToArgsMapper : IUserCardsNameXfrToArgsMapper
{
    public Task<UserCardItemsByNameExtEntitys> Map(IUserCardsNameXfrEntity source)
    {
        UserCardItemsByNameExtEntitys args = new()
        {
            UserId = source.UserId,
            CardName = source.CardName
        };

        return Task.FromResult(args);
    }
}
