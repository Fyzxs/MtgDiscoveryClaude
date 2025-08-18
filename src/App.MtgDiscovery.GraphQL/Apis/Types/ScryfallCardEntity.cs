using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Apis.Types;

public class ScryfallCardEntity
{
    // Core Card Fields
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("oracle_id")]
    public string OracleId { get; set; }

    [JsonProperty("multiverse_ids")]
    public int[] MultiverseIds { get; set; }

    [JsonProperty("mtgo_id")]
    public int? MtgoId { get; set; }

    [JsonProperty("mtgo_foil_id")]
    public int? MtgoFoilId { get; set; }

    [JsonProperty("tcgplayer_id")]
    public int? TcgplayerId { get; set; }

    [JsonProperty("cardmarket_id")]
    public int? CardmarketId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("lang")]
    public string Lang { get; set; }

    [JsonProperty("released_at")]
    public string ReleasedAt { get; set; }

    [JsonProperty("uri")]
    public string Uri { get; set; }

    [JsonProperty("scryfall_uri")]
    public string ScryfallUri { get; set; }

    [JsonProperty("layout")]
    public string Layout { get; set; }

    [JsonProperty("highres_image")]
    public bool HighresImage { get; set; }

    [JsonProperty("image_status")]
    public string ImageStatus { get; set; }

    [JsonProperty("image_uris")]
    public ImageUrisType ImageUris { get; set; }

    [JsonProperty("mana_cost")]
    public string ManaCost { get; set; }

    [JsonProperty("cmc")]
    public decimal? Cmc { get; set; }

    [JsonProperty("type_line")]
    public string TypeLine { get; set; }

    [JsonProperty("oracle_text")]
    public string OracleText { get; set; }

    [JsonProperty("colors")]
    public string[] Colors { get; set; }

    [JsonProperty("color_identity")]
    public string[] ColorIdentity { get; set; }

    [JsonProperty("keywords")]
    public string[] Keywords { get; set; }

    [JsonProperty("legalities")]
    public LegalitiesType Legalities { get; set; }

    [JsonProperty("games")]
    public string[] Games { get; set; }

    [JsonProperty("reserved")]
    public bool Reserved { get; set; }

    [JsonProperty("foil")]
    public bool Foil { get; set; }

    [JsonProperty("nonfoil")]
    public bool Nonfoil { get; set; }

    [JsonProperty("finishes")]
    public string[] Finishes { get; set; }

    [JsonProperty("oversized")]
    public bool Oversized { get; set; }

    [JsonProperty("promo")]
    public bool Promo { get; set; }

    [JsonProperty("reprint")]
    public bool Reprint { get; set; }

    [JsonProperty("variation")]
    public bool Variation { get; set; }

    [JsonProperty("set_id")]
    public string SetId { get; set; }

    [JsonProperty("set")]
    public string SetCode { get; set; }

    [JsonProperty("set_name")]
    public string SetName { get; set; }

    [JsonProperty("set_type")]
    public string SetType { get; set; }

    [JsonProperty("set_uri")]
    public string SetUri { get; set; }

    [JsonProperty("set_search_uri")]
    public string SetSearchUri { get; set; }

    [JsonProperty("scryfall_set_uri")]
    public string ScryfallSetUri { get; set; }

    [JsonProperty("rulings_uri")]
    public string RulingsUri { get; set; }

    [JsonProperty("prints_search_uri")]
    public string PrintsSearchUri { get; set; }

    [JsonProperty("collector_number")]
    public string CollectorNumber { get; set; }

    [JsonProperty("digital")]
    public bool Digital { get; set; }

    [JsonProperty("rarity")]
    public string Rarity { get; set; }

    [JsonProperty("flavor_text")]
    public string FlavorText { get; set; }

    [JsonProperty("card_back_id")]
    public string CardBackId { get; set; }

    [JsonProperty("artist")]
    public string Artist { get; set; }

    [JsonProperty("artist_ids")]
    public string[] ArtistIds { get; set; }

    [JsonProperty("illustration_id")]
    public string IllustrationId { get; set; }

    [JsonProperty("border_color")]
    public string BorderColor { get; set; }

    [JsonProperty("frame")]
    public string Frame { get; set; }

    [JsonProperty("frame_effects")]
    public string[] FrameEffects { get; set; }

    [JsonProperty("security_stamp")]
    public string SecurityStamp { get; set; }

    [JsonProperty("full_art")]
    public bool FullArt { get; set; }

    [JsonProperty("textless")]
    public bool Textless { get; set; }

    [JsonProperty("booster")]
    public bool Booster { get; set; }

    [JsonProperty("story_spotlight")]
    public bool StorySpotlight { get; set; }

    [JsonProperty("edhrec_rank")]
    public int? EdhrecRank { get; set; }

    [JsonProperty("penny_rank")]
    public int? PennyRank { get; set; }

    [JsonProperty("prices")]
    public PricesType Prices { get; set; }

    [JsonProperty("related_uris")]
    public RelatedUrisType RelatedUris { get; set; }

    [JsonProperty("purchase_uris")]
    public PurchaseUrisType PurchaseUris { get; set; }

    // Optional fields for specific layouts
    [JsonProperty("power")]
    public string Power { get; set; }

    [JsonProperty("toughness")]
    public string Toughness { get; set; }

    [JsonProperty("loyalty")]
    public string Loyalty { get; set; }

    [JsonProperty("defense")]
    public string Defense { get; set; }

    [JsonProperty("life_modifier")]
    public string LifeModifier { get; set; }

    [JsonProperty("hand_modifier")]
    public string HandModifier { get; set; }

    [JsonProperty("color_indicator")]
    public string[] ColorIndicator { get; set; }

    [JsonProperty("card_faces")]
    public CardFaceType[] CardFaces { get; set; }

    [JsonProperty("all_parts")]
    public AllPartsType[] AllParts { get; set; }

    [JsonProperty("printed_name")]
    public string PrintedName { get; set; }

    [JsonProperty("printed_type_line")]
    public string PrintedTypeLine { get; set; }

    [JsonProperty("printed_text")]
    public string PrintedText { get; set; }

    [JsonProperty("watermark")]
    public string Watermark { get; set; }

    [JsonProperty("content_warning")]
    public int? ContentWarning { get; set; }

    [JsonProperty("preview")]
    public PreviewType Preview { get; set; }

    [JsonProperty("produced_mana")]
    public string[] ProducedMana { get; set; }

    [JsonProperty("attraction_lights")]
    public string[] Attractions { get; set; }
}

public class ImageUrisType
{
    [JsonProperty("small")]
    public string Small { get; set; }

    [JsonProperty("normal")]
    public string Normal { get; set; }

    [JsonProperty("large")]
    public string Large { get; set; }

    [JsonProperty("png")]
    public string Png { get; set; }

    [JsonProperty("art_crop")]
    public string ArtCrop { get; set; }

    [JsonProperty("border_crop")]
    public string BorderCrop { get; set; }
}

public class LegalitiesType
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

public class PricesType
{
    [JsonProperty("usd")]
    public string Usd { get; set; }

    [JsonProperty("usd_foil")]
    public string UsdFoil { get; set; }

    [JsonProperty("usd_etched")]
    public string UsdEtched { get; set; }

    [JsonProperty("eur")]
    public string Eur { get; set; }

    [JsonProperty("eur_foil")]
    public string EurFoil { get; set; }

    [JsonProperty("tix")]
    public string Tix { get; set; }
}

public class RelatedUrisType
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

public class PurchaseUrisType
{
    [JsonProperty("tcgplayer")]
    public string Tcgplayer { get; set; }

    [JsonProperty("cardmarket")]
    public string Cardmarket { get; set; }

    [JsonProperty("cardhoarder")]
    public string Cardhoarder { get; set; }
}

public class CardFaceType
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
    public string[] Colors { get; set; }

    [JsonProperty("color_indicator")]
    public string[] ColorIndicator { get; set; }

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
    public ImageUrisType ImageUris { get; set; }

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

public class AllPartsType
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

public class PreviewType
{
    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("source_uri")]
    public string SourceUri { get; set; }

    [JsonProperty("previewed_at")]
    public string PreviewedAt { get; set; }
}
