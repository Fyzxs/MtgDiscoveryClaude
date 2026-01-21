using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class SealedProductsBySetIdQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.partition = @setId");
}
