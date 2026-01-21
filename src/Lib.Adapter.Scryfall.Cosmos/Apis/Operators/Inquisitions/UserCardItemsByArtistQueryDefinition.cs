using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class UserCardItemsByArtistQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.user_id = @userId AND ARRAY_CONTAINS(c.artist_ids, @artistId)");
}
