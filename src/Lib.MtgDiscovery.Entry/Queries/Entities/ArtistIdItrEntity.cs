using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class ArtistIdItrEntity : IArtistIdItrEntity
{
    public string ArtistId { get; init; }
}
