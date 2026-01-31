using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

internal sealed class AllUserWishlistCardsQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.partition = @userId");
}
