using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class ArtistNameItrEntity : IArtistNameItrEntity
{
    public string ArtistName { get; init; }
}
