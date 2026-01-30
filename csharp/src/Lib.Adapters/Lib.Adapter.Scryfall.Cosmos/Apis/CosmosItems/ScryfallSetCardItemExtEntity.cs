using Lib.Cosmos.Apis;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallSetCardItemExtEntity : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => Data.set_id;
    public dynamic Data { get; init; }
}
