using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Entities;

internal sealed class UserCardsArtistItrEntity : IUserCardsArtistItrEntity
{
    public string UserId { get; init; }
    public string ArtistId { get; init; }
}
