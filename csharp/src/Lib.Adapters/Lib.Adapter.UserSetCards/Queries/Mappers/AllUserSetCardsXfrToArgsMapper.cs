using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Adapter.UserSetCards.Queries.Mappers;

internal sealed class AllUserSetCardsXfrToArgsMapper : IAllUserSetCardsXfrToArgsMapper
{
    public Task<AllUserSetCardsExtEntitys> Map(IAllUserSetCardsXfrEntity xfr)
    {
        AllUserSetCardsExtEntitys args = new()
        {
            UserId = xfr.UserId
        };
        return Task.FromResult(args);
    }
}
