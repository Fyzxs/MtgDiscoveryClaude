using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallCardByNameItem : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => NameGuid;

    [JsonProperty("name_guid")]
    public string NameGuid { get; init; }

    [JsonProperty("data")]
    public dynamic Data { get; init; }
}
