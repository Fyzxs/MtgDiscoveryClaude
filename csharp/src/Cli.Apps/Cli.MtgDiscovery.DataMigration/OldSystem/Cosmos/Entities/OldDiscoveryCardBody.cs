using Newtonsoft.Json;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;

internal sealed class OldDiscoveryCardBody
{
    [JsonProperty("scryfall_id")]
    public required string ScryfallId { get; init; }

    [JsonProperty("set_id")]
    public required string SetId { get; init; }

    [JsonProperty("foil")]
    public required bool Foil { get; init; }

    [JsonProperty("nonfoil")]
    public required bool Nonfoil { get; init; }

    [JsonProperty("etched")]
    public required bool Etched { get; init; }
}
