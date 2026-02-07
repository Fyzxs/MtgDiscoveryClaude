using Lib.Cosmos.Apis;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.CardItems;

public sealed class ScryfallCardItemExtEntity : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => Data.id;
    public dynamic Data { get; init; }
}
