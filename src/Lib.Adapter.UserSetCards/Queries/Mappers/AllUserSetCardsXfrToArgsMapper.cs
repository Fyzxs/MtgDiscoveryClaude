using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;
using Lib.Adapter.UserSetCards.Apis.Entities;

namespace Lib.Adapter.UserSetCards.Queries.Mappers;

internal interface IAllUserSetCardsXfrToArgsMapper
{
    Task<AllUserSetCardsExtEntitys> Map(IAllUserSetCardsXfrEntity xfr);
}

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
