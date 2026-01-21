#pragma warning disable CA1056, CA1819
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Cards;

public class CardFaceOutEntity
{
    [JsonProperty("object")]
    public string ObjectString { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("mana_cost")]
    public string ManaCost { get; set; }

    [JsonProperty("type_line")]
    public string TypeLine { get; set; }

    [JsonProperty("oracle_text")]
    public string OracleText { get; set; }

    [JsonProperty("colors")]
    public ICollection<string> Colors { get; set; }

    [JsonProperty("color_indicator")]
    public ICollection<string> ColorIndicator { get; set; }

    [JsonProperty("power")]
    public string Power { get; set; }

    [JsonProperty("toughness")]
    public string Toughness { get; set; }

    [JsonProperty("loyalty")]
    public string Loyalty { get; set; }

    [JsonProperty("defense")]
    public string Defense { get; set; }

    [JsonProperty("artist")]
    public string Artist { get; set; }

    [JsonProperty("artist_id")]
    public string ArtistId { get; set; }

    [JsonProperty("illustration_id")]
    public string IllustrationId { get; set; }

    [JsonProperty("image_uris")]
    public ImageUrisOutEntity ImageUris { get; set; }

    [JsonProperty("flavor_text")]
    public string FlavorText { get; set; }

    [JsonProperty("printed_name")]
    public string PrintedName { get; set; }

    [JsonProperty("printed_type_line")]
    public string PrintedTypeLine { get; set; }

    [JsonProperty("printed_text")]
    public string PrintedText { get; set; }

    [JsonProperty("watermark")]
    public string Watermark { get; set; }

    [JsonProperty("layout")]
    public string Layout { get; set; }

    [JsonProperty("cmc")]
    public decimal? Cmc { get; set; }
}
