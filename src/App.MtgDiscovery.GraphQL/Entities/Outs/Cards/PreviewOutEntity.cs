using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Cards;

public class PreviewOutEntity
{
    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("source_uri")]
    public string SourceUri { get; set; }

    [JsonProperty("previewed_at")]
    public string PreviewedAt { get; set; }
}