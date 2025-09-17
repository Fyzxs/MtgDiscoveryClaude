using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Aggregator.Artists.Queries.Entities;
using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistNameTrigramDataExtToItrEntityMapper : IArtistNameTrigramDataExtToItrEntityMapper
{
    public Task<IArtistSearchResultItrEntity> Map([NotNull] ArtistNameTrigramDataExtEntity source)
    {
        return Task.FromResult<IArtistSearchResultItrEntity>(new ArtistSearchResultItrEntity
        {
            ArtistId = source.ArtistId,
            Name = source.Name
        });
    }
}
