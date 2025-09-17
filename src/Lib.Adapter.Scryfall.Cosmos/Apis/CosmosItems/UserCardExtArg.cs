using System.Collections.Generic;
using Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems;

public sealed class UserCardExtArg : CosmosItem
{
    public override string Id => CardId;
    public override string Partition => UserId;

    /// <summary>
    /// The unique identifier of the user who owns this card collection entry.
    /// </summary>
    [JsonProperty("user_id")]
    public string UserId { get; init; }

    /// <summary>
    /// The unique identifier of the card in the collection.
    /// </summary>
    [JsonProperty("card_id")]
    public string CardId { get; init; }

    /// <summary>
    /// The identifier of the set this card belongs to.
    /// </summary>
    [JsonProperty("set_id")]
    public string SetId { get; init; }

    /// <summary>
    /// The list of collected versions of this card with quantities and finishes.
    /// </summary>
    [JsonProperty("collected")]
    public IEnumerable<CollectedCardInfoExtArg> CollectedList { get; init; } = [];
}
