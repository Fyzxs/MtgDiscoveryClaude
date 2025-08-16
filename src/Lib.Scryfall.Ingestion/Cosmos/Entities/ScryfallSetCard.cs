using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Scryfall.Ingestion.Cosmos.Entities;

public sealed class ScryfallSetCard : CosmosItem, IScryfallPayload
{
    public override string Id => Data.id;
    public override string Partition => Data.set_id;

    [JsonProperty("data")]
    public dynamic Data { get; init; }
}
