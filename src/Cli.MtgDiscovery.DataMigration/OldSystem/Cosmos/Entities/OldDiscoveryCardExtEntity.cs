using Newtonsoft.Json;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;

public sealed class OldDiscoveryCardExtEntity
{
    [JsonProperty("id")]
    public required string id { get; init; }

    [JsonProperty("partition")]
    public required string partition { get; init; }

    [JsonProperty("body")]
    public required OldDiscoveryCardBody body { get; init; }
}
