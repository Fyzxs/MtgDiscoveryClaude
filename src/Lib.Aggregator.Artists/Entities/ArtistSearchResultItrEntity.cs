using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Entities;

internal sealed class ArtistSearchResultItrEntity : IArtistSearchResultItrEntity
{
    public string ArtistId { get; init; }
    public string Name { get; init; }
}