using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class ArtistNameItrEntity : IArtistNameItrEntity
{
    public string ArtistName { get; init; }
}
