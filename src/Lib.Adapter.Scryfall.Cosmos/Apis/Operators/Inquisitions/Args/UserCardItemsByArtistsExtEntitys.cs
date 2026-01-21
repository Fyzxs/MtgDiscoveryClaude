using System.Collections.Generic;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions.Args;

public sealed class UserCardItemsByArtistsExtEntitys
{
    public string UserId { get; init; }
    public IEnumerable<string> ArtistIds { get; init; }
}
