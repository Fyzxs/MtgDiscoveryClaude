using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class ArtistSearchResultCollectionItrToOutMapper : IArtistSearchResultCollectionItrToOutMapper
{
    public Task<List<ArtistSearchResultOutEntity>> Map(IEnumerable<IArtistSearchResultItrEntity> artistResults)
    {
        List<ArtistSearchResultOutEntity> results = artistResults.Select(artistResult => new ArtistSearchResultOutEntity { ArtistId = artistResult.ArtistId, Name = artistResult.Name }).ToList();

        return Task.FromResult(results);
    }
}
