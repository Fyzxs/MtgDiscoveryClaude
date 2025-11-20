using System.Collections.Generic;
using Lib.Shared.DataModels.Entities.Itrs;

namespace Lib.MtgDiscovery.Entry.Queries.Actions.Mappers;

internal sealed class UserCardItrEntity : IUserCardItrEntity
{
    public string UserId { get; init; }
    public string CardId { get; init; }
    public string SetId { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
    public string CardNameGuid { get; init; }
    public IUserCardDetailsItrEntity Details { get; init; }
}
