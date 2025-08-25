using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallSetCard : CosmosItem, IScryfallPayload
{
    public override string Id => Scryfall?.id ?? MtgJson?.id ?? MtgDiscovery?.id;
    public override string Partition => Scryfall?.set_id ?? MtgJson?.set_id ?? MtgDiscovery?.set_id;

    [JsonProperty("scryfall")]
    public dynamic Scryfall { get; init; }

    [JsonProperty("mtgjson")]
    public dynamic MtgJson { get; init; }

    [JsonProperty("mtgdiscovery")]
    public dynamic MtgDiscovery { get; init; }

    // Backward compatibility - remove once migration is complete
    [JsonProperty("data")]
    public dynamic Data { get; init; }
}
