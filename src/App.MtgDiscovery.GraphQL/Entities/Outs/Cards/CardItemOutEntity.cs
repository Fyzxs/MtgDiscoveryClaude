#pragma warning disable CA1056, CA1819
using System.Collections.Generic;
using Newtonsoft.Json;

namespace App.MtgDiscovery.GraphQL.Entities.Outs.Cards;

public class CardItemOutEntity
{

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("oracle_id")]
    public string OracleId { get; set; }

    [JsonProperty("arena_id")]
    public int? ArenaId { get; set; }

    [JsonProperty("mtgo_id")]
    public int? MtgoId { get; set; }

    [JsonProperty("mtgo_foil_id")]
    public int? MtgoFoilId { get; set; }

    [JsonProperty("multiverse_ids")]
    public ICollection<int> MultiverseIds { get; set; }

    [JsonProperty("tcgplayer_id")]
    public int? TcgPlayerId { get; set; }

    [JsonProperty("tcgplayer_etched_id")]
    public int? TcgPlayerEtchedId { get; set; }

    [JsonProperty("cardmarket_id")]
    public int? CardMarketId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("flavor_name")]
    public string FlavorName { get; set; }

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
    public bool HighResImage { get; set; }

    [JsonProperty("image_status")]
    public string ImageStatus { get; set; }

    [JsonProperty("image_uris")]
    public ImageUrisOutEntity ImageUris { get; set; }

    [JsonProperty("mana_cost")]
    public string ManaCost { get; set; }

    [JsonProperty("cmc")]
    public decimal? Cmc { get; set; }

    [JsonProperty("type_line")]
    public string TypeLine { get; set; }

    [JsonProperty("oracle_text")]
    public string OracleText { get; set; }

    [JsonProperty("colors")]
    public ICollection<string> Colors { get; set; }

    [JsonProperty("color_identity")]
    public ICollection<string> ColorIdentity { get; set; }

    [JsonProperty("keywords")]
    public ICollection<string> Keywords { get; set; }

    [JsonProperty("legalities")]
    public LegalitiesOutEntity Legalities { get; set; }

    [JsonProperty("games")]
    public ICollection<string> Games { get; set; }

    [JsonProperty("reserved")]
    public bool Reserved { get; set; }

    [JsonProperty("game_changer")]
    public bool GameChanger { get; set; }

    [JsonProperty("foil")]
    public bool Foil { get; set; }

    [JsonProperty("nonfoil")]
    public bool NonFoil { get; set; }

    [JsonProperty("finishes")]
    public ICollection<string> Finishes { get; set; }

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
    public ICollection<string> ArtistIds { get; set; }

    [JsonProperty("illustration_id")]
    public string IllustrationId { get; set; }

    [JsonProperty("border_color")]
    public string BorderColor { get; set; }

    [JsonProperty("frame")]
    public string Frame { get; set; }

    [JsonProperty("frame_effects")]
    public ICollection<string> FrameEffects { get; set; }

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

    [JsonProperty("promo_types")]
    public ICollection<string> PromoTypes { get; set; }

    [JsonProperty("edhrec_rank")]
    public int? EdhRecRank { get; set; }

    [JsonProperty("penny_rank")]
    public int? PennyRank { get; set; }

    [JsonProperty("prices")]
    public PricesOutEntity Prices { get; set; }

    [JsonProperty("related_uris")]
    public RelatedUrisOutEntity RelatedUris { get; set; }

    [JsonProperty("purchase_uris")]
    public PurchaseUrisOutEntity PurchaseUris { get; set; }

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
    public ICollection<string> ColorIndicator { get; set; }

    [JsonProperty("card_faces")]
    public ICollection<CardFaceOutEntity> CardFaces { get; set; }

    [JsonProperty("all_parts")]
    public ICollection<AllPartsOutEntity> AllParts { get; set; }

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
    public PreviewOutEntity Preview { get; set; }

    [JsonProperty("produced_mana")]
    public ICollection<string> ProducedMana { get; set; }

    [JsonProperty("attraction_lights")]
    public ICollection<int> AttractionLights { get; set; }
}
