using System.Collections.ObjectModel;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Nesteds;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class CardNameTrigramItem : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => Trigram;

    [JsonProperty("partition")]
    public override string Partition => Trigram[..1];

    [JsonProperty("trigram")]
    public string Trigram { get; init; }

    [JsonProperty("cards")]
    public Collection<CardNameTrigramDataItem> Cards { get; init; }
}
