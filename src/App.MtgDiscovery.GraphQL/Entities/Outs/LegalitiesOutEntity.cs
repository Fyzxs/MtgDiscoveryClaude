using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs;

public class LegalitiesOutEntity
{
    [JsonProperty("standard")]
    public string Standard { get; set; }

    [JsonProperty("future")]
    public string Future { get; set; }

    [JsonProperty("historic")]
    public string Historic { get; set; }

    [JsonProperty("timeless")]
    public string Timeless { get; set; }

    [JsonProperty("gladiator")]
    public string Gladiator { get; set; }

    [JsonProperty("pioneer")]
    public string Pioneer { get; set; }

    [JsonProperty("explorer")]
    public string Explorer { get; set; }

    [JsonProperty("modern")]
    public string Modern { get; set; }

    [JsonProperty("legacy")]
    public string Legacy { get; set; }

    [JsonProperty("pauper")]
    public string Pauper { get; set; }

    [JsonProperty("vintage")]
    public string Vintage { get; set; }

    [JsonProperty("penny")]
    public string Penny { get; set; }

    [JsonProperty("commander")]
    public string Commander { get; set; }

    [JsonProperty("oathbreaker")]
    public string Oathbreaker { get; set; }

    [JsonProperty("standardbrawl")]
    public string StandardBrawl { get; set; }

    [JsonProperty("brawl")]
    public string Brawl { get; set; }

    [JsonProperty("alchemy")]
    public string Alchemy { get; set; }

    [JsonProperty("paupercommander")]
    public string PauperCommander { get; set; }

    [JsonProperty("duel")]
    public string Duel { get; set; }

    [JsonProperty("oldschool")]
    public string Oldschool { get; set; }

    [JsonProperty("premodern")]
    public string Premodern { get; set; }

    [JsonProperty("predh")]
    public string Predh { get; set; }
}