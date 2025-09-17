using Lib.Cosmos.Apis;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallSetItemExtEntity : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => Data.id;
    public dynamic Data { get; init; }
}
