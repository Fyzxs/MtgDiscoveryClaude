#pragma warning disable CA1056
using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Cards;

public class AllPartsOutEntity
{
    [JsonProperty("object")]
    public string ObjectString { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("component")]
    public string Component { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("type_line")]
    public string TypeLine { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }
}