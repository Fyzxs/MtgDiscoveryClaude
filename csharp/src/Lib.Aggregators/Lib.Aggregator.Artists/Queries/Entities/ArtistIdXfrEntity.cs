using Lib.Adapter.Artists.Apis.Entities;

namespace Lib.Aggregator.Artists.Queries.Entities;

internal sealed class ArtistIdXfrEntity : IArtistIdXfrEntity
{
    public string ArtistId { get; init; }
}
