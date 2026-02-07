using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class AccessibleCollectionsQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.owner_id = @userId OR EXISTS (SELECT VALUE au FROM au IN c.authorized_users WHERE au.user_id = @userId)");
}
