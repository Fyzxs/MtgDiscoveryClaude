using System.Collections.ObjectModel;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class CardNameTrigramExtArg : CosmosItem
{
    [JsonProperty("id")]
    public override string Id => Trigram;

    [JsonProperty("partition")]
    public override string Partition => Trigram[..1];

    [JsonProperty("trigram")]
    public string Trigram { get; init; }

    [JsonProperty("cards")]
    public Collection<CardNameTrigramDataExtArg> Cards { get; init; }
}
