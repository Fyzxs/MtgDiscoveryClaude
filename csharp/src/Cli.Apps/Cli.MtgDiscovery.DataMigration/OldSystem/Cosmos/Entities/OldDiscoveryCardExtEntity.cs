using Newtonsoft.Json;

namespace Cli.MtgDiscovery.DataMigration.OldSystem.Cosmos.Entities;

internal sealed class OldDiscoveryCardExtEntity
{
    [JsonProperty("id")]
    public required string Id { get; init; }

    [JsonProperty("partition")]
    public required string Partition { get; init; }

    [JsonProperty("body")]
    public required OldDiscoveryCardBody Body { get; init; }
}
