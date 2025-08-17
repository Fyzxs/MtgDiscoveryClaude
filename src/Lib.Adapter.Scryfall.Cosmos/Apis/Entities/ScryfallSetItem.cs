using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallSetItem : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => Data.code;

    [JsonProperty("data")]
    public dynamic Data { get; init; }
}
