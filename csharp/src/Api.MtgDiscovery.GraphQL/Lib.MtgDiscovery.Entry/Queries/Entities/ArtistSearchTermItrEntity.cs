using Lib.Shared.DataModels.Entities.Itrs.Artists;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class ArtistSearchTermItrEntity : IArtistSearchTermItrEntity
{
    public string SearchTerm { get; init; }
}
