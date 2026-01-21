using Lib.Cosmos.Apis.Operators;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Operators.Inquisitions;

public sealed class UserCardItemsByNameQueryDefinition : InquiryDefinition
{
    public override QueryDefinition AsSystemType() => new("SELECT * FROM c WHERE c.partition = @userId AND c.card_name_guid = @cardNameGuid");
}
