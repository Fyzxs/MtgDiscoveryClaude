using Lib.Cosmos.Apis.Operators;
using Microsoft.Azure.Cosmos;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class CollectionByIdQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.id = @collectionId");
}
