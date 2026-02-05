using System.Collections.Generic;
using Lib.Cosmos.Apis;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.UserCards;

public sealed class UserCardExtEntity : CosmosItem
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
    /// The name of the card (denormalized for display without additional lookups).
    /// </summary>
    [JsonProperty("card_name")]
    public string CardName { get; init; }

    /// <summary>
    /// The name of the set (denormalized for display without additional lookups).
    /// </summary>
    [JsonProperty("set_name")]
    public string SetName { get; init; }

    /// <summary>
    /// The code of the set (denormalized for URLs and display).
    /// </summary>
    [JsonProperty("set_code")]
    public string SetCode { get; init; }

    /// <summary>
    /// The release date of the card in ISO format (denormalized for sorting).
    /// </summary>
    [JsonProperty("released_at")]
    public string ReleasedAt { get; init; }

    /// <summary>
    /// The artist name (denormalized for display without additional lookups).
    /// </summary>
    [JsonProperty("artist_name")]
    public string Artist { get; init; }

    /// <summary>
    /// The artist IDs for this card (used for efficient querying by artist).
    /// </summary>
    [JsonProperty("artist_ids")]
    public IEnumerable<string> ArtistIds { get; init; } = [];

    /// <summary>
    /// The deterministic GUID generated from the card name (used for efficient querying by name).
    /// This matches the NameGuid used in the CardsByName collection.
    /// </summary>
    [JsonProperty("card_name_guid")]
    public string CardNameGuid { get; init; }

    /// <summary>
    /// The list of collected versions of this card with quantities and finishes.
    /// </summary>
    [JsonProperty("collected")]
    public IEnumerable<UserCardDetailsExtEntity> CollectedList { get; init; } = [];

    [JsonProperty("collection_id")]
    public string CollectionId { get; init; } = string.Empty;
}
