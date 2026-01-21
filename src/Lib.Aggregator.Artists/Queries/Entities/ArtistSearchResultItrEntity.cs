using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistSearchResultItrEntity : IArtistSearchResultItrEntity
{
    public string ArtistId { get; init; }
    public string Name { get; init; }
}
