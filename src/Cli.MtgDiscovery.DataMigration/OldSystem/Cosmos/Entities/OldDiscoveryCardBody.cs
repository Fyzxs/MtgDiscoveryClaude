using Newtonsoft.Json;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;

public sealed class OldDiscoveryCardBody
{
    [JsonProperty("scryfall_id")]
    public required string scryfall_id { get; init; }

    [JsonProperty("set_id")]
    public required string set_id { get; init; }

    [JsonProperty("foil")]
    public required bool foil { get; init; }

    [JsonProperty("nonfoil")]
    public required bool nonfoil { get; init; }

    [JsonProperty("etched")]
    public required bool etched { get; init; }
}
