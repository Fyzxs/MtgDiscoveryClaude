using Lib.Shared.DataModels.Entities.Oufs.Artists;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistSearchResultOufEntity : IArtistSearchResultOufEntity
{
    public string ArtistId { get; init; }
    public string Name { get; init; }
}
