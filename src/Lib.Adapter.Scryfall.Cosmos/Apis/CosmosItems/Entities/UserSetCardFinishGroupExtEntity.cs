using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Adapter.Scryfall.Cosmos.Apis.CosmosItems.Entities;

public sealed class UserSetCardFinishGroupExtEntity
{
    [JsonProperty("cards")]
    public ICollection<string> Cards { get; init; } = [];
}
