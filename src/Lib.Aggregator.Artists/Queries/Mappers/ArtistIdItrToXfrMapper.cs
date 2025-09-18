using System.Threading.Tasks;
using Lib.Adapter.Artists.Apis.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistIdItrToXfrMapper : IArtistIdItrToXfrMapper
{
    public Task<IArtistIdXfrEntity> Map(IArtistIdItrEntity source)
    {
        return Task.FromResult<IArtistIdXfrEntity>(new ArtistIdXfrEntity
        {
            ArtistId = source.ArtistId
        });
    }
}
