using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.UserCards.Queries.Mappers;

internal sealed class UserCardsArtistItrToXfrMapper : IUserCardsArtistItrToXfrMapper
{
    public Task<IUserCardsArtistXfrEntity> Map(IUserCardsArtistItrEntity source)
    {
        return Task.FromResult<IUserCardsArtistXfrEntity>(new UserCardsArtistXfrEntity
        {
            UserId = source.UserId,
            ArtistId = source.ArtistId
        });
    }
}
