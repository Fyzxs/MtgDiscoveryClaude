using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class ScryfallSetCodeIndexExtArg : CosmosItem
{
    public override string Id => SetCode;
    public override string Partition => SetCode;

    [JsonProperty("set_code")]
    public string SetCode { get; init; }

    [JsonProperty("set_id")]
    public string SetId { get; init; }
}
