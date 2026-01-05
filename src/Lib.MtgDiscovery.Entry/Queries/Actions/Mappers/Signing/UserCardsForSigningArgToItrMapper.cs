using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args.UserCards;
using Lib.Shared.DataModels.Entities.Itrs.UserCards;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers.Signing;

internal sealed class UserCardsForSigningArgToItrMapper : IUserCardsForSigningArgToItrMapper
{
    public Task<IUserCardsForSigningItrEntity> Map(IUserCardsForSigningArgEntity argEntity)
    {
        return Task.FromResult<IUserCardsForSigningItrEntity>(new UserCardsForSigningItrEntity
        {
            UserId = argEntity.UserId,
            ArtistIds = argEntity.ArtistIds
        });
    }
}
