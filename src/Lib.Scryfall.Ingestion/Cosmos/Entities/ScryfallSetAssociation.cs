using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Cosmos.Entities;

public sealed class ScryfallSetAssociation : CosmosItem
{
    public override string Id => SetId;
    public override string Partition => ParentSetCode;

    [JsonProperty("setId")]
    public string SetId { get; init; }

    [JsonProperty("parentSetCode")]
    public string ParentSetCode { get; init; }

    [JsonProperty("setCode")]
    public string SetCode { get; init; }

    [JsonProperty("setName")]
    public string SetName { get; init; }
}
