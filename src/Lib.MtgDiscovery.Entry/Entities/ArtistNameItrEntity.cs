using Lib.Shared.DataModels.Entities;

namespace Lib.MtgDiscovery.Entry.Entities;

internal sealed class ArtistNameItrEntity : IArtistNameItrEntity
{
    public string ArtistName { get; init; }
}
