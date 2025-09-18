using Lib.Shared.DataModels.Entities;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class ArtistIdItrEntity : IArtistIdItrEntity
{
    public string ArtistId { get; init; }
}
