using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.Entities;

public sealed class ScryfallCardByName : CosmosItem, IScryfallPayload
{
    public override string Id => Scryfall?.id ?? MtgJson?.id ?? MtgDiscovery?.id;
    public override string Partition => NameGuid;

    [JsonProperty("name_guid")]
    public string NameGuid { get; init; }

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