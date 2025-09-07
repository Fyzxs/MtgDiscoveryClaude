using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallSetCardItem : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => Data.set_id;

    [JsonProperty("data")]
    public dynamic Data { get; init; }
}
