#pragma warning disable CA1056, CA1819
using System.Collections.Generic;

namespace Lib.Shared.DataModels.Entities.Outs.Cards;

public class CardItemOutEntity
{

    public string Id { get; set; }

    public string OracleId { get; set; }

    public int? ArenaId { get; set; }

    public int? MtgoId { get; set; }

    public int? MtgoFoilId { get; set; }

    public ICollection<int> MultiverseIds { get; set; }

    public int? TcgPlayerId { get; set; }

    public int? TcgPlayerEtchedId { get; set; }

    public int? CardMarketId { get; set; }

    public string Name { get; set; }

    public string FlavorName { get; set; }

    public string Lang { get; set; }

    public string ReleasedAt { get; set; }

    public string Uri { get; set; }

    public string ScryfallUri { get; set; }

    public string Layout { get; set; }

    public bool HighResImage { get; set; }

    public string ImageStatus { get; set; }

    public ImageUrisOutEntity ImageUris { get; set; }

    public string ManaCost { get; set; }

    public decimal? Cmc { get; set; }

    public string TypeLine { get; set; }

    public string OracleText { get; set; }

    public ICollection<string> Colors { get; set; }

    public ICollection<string> ColorIdentity { get; set; }

    public ICollection<string> Keywords { get; set; }

    public LegalitiesOutEntity Legalities { get; set; }

    public ICollection<string> Games { get; set; }

    public bool Reserved { get; set; }

    public bool GameChanger { get; set; }

    public bool Foil { get; set; }

    public bool NonFoil { get; set; }

    public ICollection<string> Finishes { get; set; }

    public bool Oversized { get; set; }

    public bool Promo { get; set; }

    public bool Reprint { get; set; }

    public bool Variation { get; set; }

    public string SetId { get; set; }

    public string SetCode { get; set; }

    public string SetName { get; set; }

    public string SetType { get; set; }

    public string SetUri { get; set; }

    public string SetSearchUri { get; set; }

    public string ScryfallSetUri { get; set; }

    public string RulingsUri { get; set; }

    public string PrintsSearchUri { get; set; }

    public string CollectorNumber { get; set; }

    public bool Digital { get; set; }

    public string Rarity { get; set; }

    public string FlavorText { get; set; }

    public string CardBackId { get; set; }

    public string Artist { get; set; }

    public ICollection<string> ArtistIds { get; set; }

    public string IllustrationId { get; set; }

    public string BorderColor { get; set; }

    public string Frame { get; set; }

    public ICollection<string> FrameEffects { get; set; }

    public string SecurityStamp { get; set; }

    public bool FullArt { get; set; }

    public bool Textless { get; set; }

    public bool Booster { get; set; }

    public bool StorySpotlight { get; set; }

    public ICollection<string> PromoTypes { get; set; }

    public int? EdhRecRank { get; set; }

    public int? PennyRank { get; set; }

    public PricesOutEntity Prices { get; set; }

    public RelatedUrisOutEntity RelatedUris { get; set; }

    public PurchaseUrisOutEntity PurchaseUris { get; set; }

    // Optional fields for specific layouts

    public string Power { get; set; }

    public string Toughness { get; set; }

    public string Loyalty { get; set; }

    public string Defense { get; set; }

    public string LifeModifier { get; set; }

    public string HandModifier { get; set; }

    public ICollection<string> ColorIndicator { get; set; }

    public ICollection<CardFaceOutEntity> CardFaces { get; set; }

    public ICollection<AllPartsOutEntity> AllParts { get; set; }

    public string PrintedName { get; set; }

    public string PrintedTypeLine { get; set; }

    public string PrintedText { get; set; }

    public string Watermark { get; set; }

    public int? ContentWarning { get; set; }

    public PreviewOutEntity Preview { get; set; }

    public ICollection<string> ProducedMana { get; set; }

    public ICollection<int> AttractionLights { get; set; }
}
