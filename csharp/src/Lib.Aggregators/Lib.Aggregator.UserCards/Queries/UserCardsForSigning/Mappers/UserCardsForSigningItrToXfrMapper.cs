using System.Threading.Tasks;
using Lib.Adapter.UserCards.Apis.Entities;
using Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Entities;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.Aggregator.UserCards.Queries.UserCardsForSigning.Mappers;

internal sealed class UserCardsForSigningItrToXfrMapper : IUserCardsForSigningItrToXfrMapper
{
    public Task<IUserCardsForSigningXfrEntity> Map(IUserCardsForSigningItrEntity source)
    {
        IUserCardsForSigningXfrEntity xfrEntity = new UserCardsForSigningXfrEntity
        {
            UserId = source.UserId,
            ArtistIds = source.ArtistIds
        };

        return Task.FromResult(xfrEntity);
    }
}
