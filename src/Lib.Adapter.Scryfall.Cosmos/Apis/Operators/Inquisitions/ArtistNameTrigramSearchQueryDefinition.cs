using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class ArtistNameTrigramSearchQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.id = @trigram AND c.partition = @partition AND EXISTS(SELECT VALUE artist FROM artist IN c.artists WHERE CONTAINS(artist.norm, @normalized))");
}
