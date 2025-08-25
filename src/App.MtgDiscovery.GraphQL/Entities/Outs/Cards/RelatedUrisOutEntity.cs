using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Cards;

public class RelatedUrisOutEntity
{
    [JsonProperty("gatherer")]
    public string Gatherer { get; set; }

    [JsonProperty("tcgplayer_infinite_articles")]
    public string TcgplayerInfiniteArticles { get; set; }

    [JsonProperty("tcgplayer_infinite_decks")]
    public string TcgplayerInfiniteDecks { get; set; }

    [JsonProperty("edhrec")]
    public string Edhrec { get; set; }
}