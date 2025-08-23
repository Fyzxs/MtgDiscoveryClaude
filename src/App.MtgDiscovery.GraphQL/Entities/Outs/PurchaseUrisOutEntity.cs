using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs;

public class PurchaseUrisOutEntity
{
    [JsonProperty("tcgplayer")]
    public string Tcgplayer { get; set; }

    [JsonProperty("cardmarket")]
    public string Cardmarket { get; set; }

    [JsonProperty("cardhoarder")]
    public string Cardhoarder { get; set; }
}