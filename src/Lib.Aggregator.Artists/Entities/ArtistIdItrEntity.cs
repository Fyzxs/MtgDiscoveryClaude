using Lib.Shared.DataModels.Entities;

namespace Lib.Aggregator.Artists.Entities;

internal sealed class ArtistIdItrEntity : IArtistIdItrEntity
{
    public string ArtistId { get; init; }
}
