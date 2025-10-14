using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lib.MtgDiscovery.Entry.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Mappers;

internal sealed class ArtistSearchResultCollectionOufToOutMapper : IArtistSearchResultCollectionOufToOutMapper
{
    public Task<List<ArtistSearchResultOutEntity>> Map(IArtistSearchResultCollectionOufEntity collection)
    {
        List<ArtistSearchResultOutEntity> results = [.. collection.Artists.Select(artistResult => new ArtistSearchResultOutEntity { ArtistId = artistResult.ArtistId, Name = artistResult.Name })];

        return Task.FromResult(results);
    }
}
