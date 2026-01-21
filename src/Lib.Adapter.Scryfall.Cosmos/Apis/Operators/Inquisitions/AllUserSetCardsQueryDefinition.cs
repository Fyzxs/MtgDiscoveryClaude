using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

internal sealed class AllUserSetCardsQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() =>
        new("SELECT * FROM c WHERE c.user_id = @userId");
}
