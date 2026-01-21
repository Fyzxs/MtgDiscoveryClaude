using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class ArtistSearchTermItrEntity : IArtistSearchTermItrEntity
{
    public string SearchTerm { get; init; }
}
