using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Entities;
using Lib.Adapter.UserCards.Apis.Entities;

namespace Lib.Adapter.UserCards.Queries.Mappers;

internal sealed class UserCardsForSigningXfrToArgsMapper : IUserCardsForSigningXfrToArgsMapper
{
    public Task<UserCardItemsByArtistsExtEntitys> Map(IUserCardsForSigningXfrEntity source)
    {
        UserCardItemsByArtistsExtEntitys args = new()
        {
            UserId = source.UserId,
            ArtistIds = source.ArtistIds
        };

        return Task.FromResult(args);
    }
}
