using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.ArtistNameTrigrams;
using Lib.Aggregator.Artists.Entities;
using Lib.Shared.DataModels.Entities.Oufs.Artists;

namespace Lib.Aggregator.Artists.Queries.Mappers;

internal sealed class ArtistNameTrigramDataExtToOufEntityMapper : IArtistNameTrigramDataExtToOufEntityMapper
{
    public Task<IArtistSearchResultOufEntity> Map([NotNull] ArtistNameTrigramDataExtEntity source)
    {
        return Task.FromResult<IArtistSearchResultOufEntity>(new ArtistSearchResultOufEntity
        {
            ArtistId = source.ArtistId,
            Name = source.Name
        });
    }
}
