using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Queries.Entities;
using Lib.Shared.DataModels.Entities.Args;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistIdArgToUserCardsArtistContextMapper : IArtistIdArgToUserCardsArtistContextMapper
{
    public Task<IUserCardsArtistItrEntity> Map(IArtistIdArgEntity artistId)
    {
        IUserCardsArtistItrEntity context = new UserCardsArtistItrEntity
        {
            UserId = artistId.UserId,
            ArtistId = artistId.ArtistId
        };

        return Task.FromResult(context);
    }
}
