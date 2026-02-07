using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal sealed class UserCardsArtistXfrToArgsMapper : IUserCardsArtistXfrToArgsMapper
{
    public Task<UserCardItemsByArtistExtEntity> Map(IUserCardsArtistXfrEntity source)
    {
        UserCardItemsByArtistExtEntity args = new()
        {
            UserId = source.UserId,
            ArtistId = source.ArtistId
        };

        return Task.FromResult(args);
    }
}
