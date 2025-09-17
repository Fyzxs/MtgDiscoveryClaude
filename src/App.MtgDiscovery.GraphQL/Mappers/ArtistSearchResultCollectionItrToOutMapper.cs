using System.Collections.Generic;
using System.Threading.Tasks;
using App.MtgDiscovery.GraphQL.Entities.Outs.Artists;
using Lib.Shared.DataModels.Entities;

namespace App.MtgDiscovery.GraphQL.Mappers;

internal sealed class ArtistSearchResultCollectionItrToOutMapper : IArtistSearchResultCollectionItrToOutMapper
{
    public Task<List<ArtistSearchResultOutEntity>> Map(IEnumerable<IArtistSearchResultItrEntity> artistResults)
    {
        List<ArtistSearchResultOutEntity> results = [];

        foreach (IArtistSearchResultItrEntity artistResult in artistResults)
        {
            results.Add(new ArtistSearchResultOutEntity { ArtistId = artistResult.ArtistId, Name = artistResult.Name });
        }

        return Task.FromResult(results);
    }
}
