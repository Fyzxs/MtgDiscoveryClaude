using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class UserCardItemsByArtistsQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.partition = @userId AND EXISTS(SELECT VALUE a FROM a IN c.artist_ids WHERE ARRAY_CONTAINS(@artistIds, a))");
}
